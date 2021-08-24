using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Core.Models;
using YuGet.Core.Models.Abstraction;
using YuGet.Database;
using YuGet.Database.Models;

namespace YuGet.Core
{
	public class DatabaseSearchService : ISearchService
    {
        private readonly IYuGetDbContext _context;
        private readonly IFrameworkCompatibilityService _frameworks;
        private readonly IUrlGenerator _url;

        public DatabaseSearchService(IYuGetDbContext context, IFrameworkCompatibilityService frameworks, IUrlGenerator url)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _frameworks = frameworks ?? throw new ArgumentNullException(nameof(frameworks));
            _url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public async Task<SearchResponse> SearchAsync(
            SearchRequest request,
            CancellationToken cancellationToken)
        {
            var result = new List<SearchResult>();
            var packages = await SearchImplAsync(request, cancellationToken);

            foreach (var package in packages)
            {
                var versions = package.OrderByDescending(p => p.Version).ToList();
                var latest = versions.First();
                var iconUrl = latest.HasEmbeddedIcon
                    ? _url.GetPackageIconDownloadUrl(latest.Key, latest.Version)
                    : latest.IconUrlString;

                result.Add(new SearchResultRef
                {
                    PackageId = latest.Key,
                    Version = latest.Version.ToFullString(),
                    Description = latest.Description,
                    Authors = latest.Authors,
                    IconUrl = iconUrl,
                    LicenseUrl = latest.LicenseUrlString,
                    ProjectUrl = latest.ProjectUrlString,
                    RegistrationIndexUrl = _url.GetRegistrationIndexUrl(latest.Key),
                    Summary = latest.Summary,
                    Tags = latest.Tags.Select(a => a.Tag.Name).ToArray(),
                    Title = latest.Title,
                    TotalDownloads = versions.Sum(p => p.Downloads),
                    Versions = versions
                        .Select(p => new SearchResultVersionRef
                        {
                            RegistrationLeafUrl = _url.GetRegistrationLeafUrl(p.Key, p.Version),
                            Version = p.Version.ToFullString(),
                            Downloads = p.Downloads,
                        })
                        .ToList()
                });
            }

            return new SearchResponseRef
            {
                TotalHits = result.Count,
                Data = result,
                Context = SearchContextRef.Default(_url.GetPackageMetadataResourceUrl())
            };
        }

        public async Task<AutocompleteResponse> AutocompleteAsync(
            AutocompleteRequest request,
            CancellationToken cancellationToken)
        {
            IQueryable<Package> search = _context.Set<Package>();

            if (!string.IsNullOrEmpty(request.Query))
            {
                var query = request.Query.ToLower();
                search = search.Where(p => p.Key.ToLower().Contains(query));
            }

            search = AddSearchFilters(
                search,
                request.IncludePrerelease,
                request.IncludeSemVer2,
                request.PackageType,
                frameworks: null);

            var results = await search
                .OrderByDescending(p => p.Downloads)
                .Distinct()
                .Skip(request.Skip)
                .Take(request.Take)
                .Select(p => p.Key)
                .ToListAsync(cancellationToken);

            return new AutocompleteResponseRef
            {
                TotalHits = results.Count,
                Data = results,
                Context = AutocompleteContextRef.Default
            };
        }

        public async Task<AutocompleteResponse> ListPackageVersionsAsync(
            VersionsRequest request,
            CancellationToken cancellationToken)
        {
            var packageId = request.PackageId.ToLower();
            IQueryable<Package> search = _context
                .Set<Package>()
                .Where(p => p.Key.ToLower().Equals(packageId));

            search = AddSearchFilters(
                search,
                request.IncludePrerelease,
                request.IncludeSemVer2,
                packageType: null,
                frameworks: null);

            var results = await search
                .Select(p => p.NormalizedVersionString)
                .ToListAsync(cancellationToken);

            return new AutocompleteResponseRef
            {
                TotalHits = results.Count,
                Data = results,
                Context = AutocompleteContextRef.Default
            };
        }

        public async Task<DependentsResponse> FindDependentsAsync(string packageKey, CancellationToken cancellationToken)
        {
            var results = await _context
                .Set<Package>()
                .Where(p => p.Listed)
                .OrderByDescending(p => p.Downloads)
                .Where(p => p.Dependencies.Any(d => d.Key == packageKey))
                .Take(20)
                .Select(r => new DependentResultRef
                {
                    Id = r.Key,
                    Description = r.Description,
                    TotalDownloads = r.Downloads
                })
                .Distinct()
                .ToListAsync(cancellationToken);

            return new DependentsResponseRef
            {
                TotalHits = results.Count,
                Data = results
            };
        }

        private async Task<List<IGrouping<string, Package>>> SearchImplAsync(
            SearchRequest request,
            CancellationToken cancellationToken)
        {
            var frameworks = GetCompatibleFrameworksOrNull(request.Framework);
            IQueryable<Package> search = _context.Set<Package>();

            search = AddSearchFilters(
                search,
                request.IncludePrerelease,
                request.IncludeSemVer2,
                request.PackageType,
                frameworks);

            if (!string.IsNullOrEmpty(request.Query))
            {
                var query = request.Query.ToLower();
                search = search.Where(p => p.Key.ToLower().Contains(query));
            }

            var packageIds = search.Select(p => p.Id)
                .Distinct()
                .OrderBy(id => id)
                .Skip(request.Skip)
                .Take(request.Take);

            // This query MUST fetch all versions for each package that matches the search,
            // otherwise the results for a package's latest version may be incorrect.
            // If possible, we'll find all these packages in a single query by matching
            // the package IDs in a subquery. Otherwise, run two queries:
            //   1. Find the package IDs that match the search
            //   2. Find all package versions for these package IDs
            if (_context.SupportsLimitInSubqueries)
            {
                search = _context.Set<Package>().Where(p => packageIds.Contains(p.Id));
            }
            else
            {
                var packageIdResults = await packageIds.ToListAsync(cancellationToken);

                search = _context.Set<Package>().Where(p => packageIdResults.Contains(p.Id));
            }

            search = AddSearchFilters(
                search,
                request.IncludePrerelease,
                request.IncludeSemVer2,
                request.PackageType,
                frameworks);

            var results = await search.ToListAsync(cancellationToken);

            return results.GroupBy(p => p.Key).ToList();
        }

        private IQueryable<Package> AddSearchFilters(
            IQueryable<Package> query,
            bool includePrerelease,
            bool includeSemVer2,
            string packageType,
            IReadOnlyList<string> frameworks)
        {
            if (!includePrerelease)
            {
                query = query.Where(p => !p.IsPrerelease);
            }

            if (!includeSemVer2)
            {
                query = query.Where(p => p.SemVerLevel != SemVerLevel.SemVer2);
            }

            if (!string.IsNullOrEmpty(packageType))
            {
                query = query.Where(p => p.PackageTypes.Any(t => t.Name == packageType));
            }

            if (frameworks != null)
            {
                query = query.Where(p => p.TargetFrameworks.Any(f => frameworks.Contains(f.Moniker)));
            }

            query = query.Where(p => p.Listed);

            return query;
        }

        private IReadOnlyList<string> GetCompatibleFrameworksOrNull(string framework)
        {
            if (framework == null) return null;

            return _frameworks.FindAllCompatibleFrameworks(framework);
        }
    }
}
