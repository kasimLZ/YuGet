using System;
using System.Collections.Generic;
using System.Linq;
using YuGet.Core.Models;
using YuGet.Core.Models.Abstraction;
using YuGet.Database.Models;

namespace YuGet.Core
{
	public class RegistrationBuilder
    {
        private readonly IUrlGenerator _url;

        public RegistrationBuilder(IUrlGenerator url)
        {
            _url = url ?? throw new ArgumentNullException(nameof(url));
        }

        public virtual DefaultRegistrationIndexResponse BuildIndex(PackageRegistration registration)
        {
            var sortedPackages = registration.Packages.OrderBy(p => p.Version).ToList();

            // TODO: Paging of registration items.
            // "Un-paged" example: https://api.nuget.org/v3/registration3/newtonsoft.json/index.json
            // Paged example: https://api.nuget.org/v3/registration3/fake/index.json
            return new DefaultRegistrationIndexResponse
            {
                RegistrationIndexUrl = _url.GetRegistrationIndexUrl(registration.PackageId),
                Type = RegistrationIndexResponseRef.DefaultType,
                Count = 1,
                TotalDownloads = registration.Packages.Sum(p => p.Downloads),
                Pages = new[]
                {
                    new DefaultRegistrationIndexPage
                    {
                        RegistrationPageUrl = _url.GetRegistrationIndexUrl(registration.PackageId),
                        Count = registration.Packages.Count(),
                        Lower = sortedPackages.First().Version.ToNormalizedString().ToLowerInvariant(),
                        Upper = sortedPackages.Last().Version.ToNormalizedString().ToLowerInvariant(),
                        ItemsOrNull = sortedPackages.Select(ToRegistrationIndexPageItem).ToList(),
                    }
                }
            };
        }

        public virtual RegistrationLeafResponse BuildLeaf(Package package)
        {
            var version = package.Version;

            return new RegistrationLeafResponseRef
            {
                Type = RegistrationLeafResponseRef.DefaultType,
                Listed = package.Listed,
                Published = package.Published,
                RegistrationLeafUrl = _url.GetRegistrationLeafUrl(package.Key, version),
                PackageContentUrl = _url.GetPackageDownloadUrl(package.Key, version),
                RegistrationIndexUrl = _url.GetRegistrationIndexUrl(package.Key)
            };
        }

        private DefaultRegistrationIndexPageItem ToRegistrationIndexPageItem(Package package) =>
            new DefaultRegistrationIndexPageItem
            {
                RegistrationLeafUrl = _url.GetRegistrationLeafUrl(package.Key, package.Version),
                PackageContentUrl = _url.GetPackageDownloadUrl(package.Key, package.Version),
                PackageMetadata = new DefaultPackageMetadata
                {
                    PackageId = package.Key,
                    Version = package.Version.ToFullString(),
                    Authors = string.Join(", ", package.Authors),
                    Description = package.Description,
                    Downloads = package.Downloads,
                    HasReadme = package.HasReadme,
                    IconUrl = package.HasEmbeddedIcon
                        ? _url.GetPackageIconDownloadUrl(package.Key, package.Version)
                        : package.IconUrlString,
                    Language = package.Language,
                    LicenseUrl = package.LicenseUrlString,
                    Listed = package.Listed,
                    MinClientVersion = package.MinClientVersion,
                    ReleaseNotes = package.ReleaseNotes,
                    PackageContentUrl = _url.GetPackageDownloadUrl(package.Key, package.Version),
                    PackageTypes = package.PackageTypes.Select(t => t.Name).ToList(),
                    ProjectUrl = package.ProjectUrlString,
                    RepositoryUrl = package.RepositoryUrlString,
                    RepositoryType = package.RepositoryType,
                    Published = package.Published,
                    RequireLicenseAcceptance = package.RequireLicenseAcceptance,
                    Summary = package.Summary,
                    Tags = package.Tags.Select(a => a.Tag.Name).ToArray(),
                    Title = package.Title,
                    DependencyGroups = ToDependencyGroups(package)
                },
            };

        private static IReadOnlyList<DependencyGroupItem> ToDependencyGroups(Package package)
        {
            return package.Dependencies
                .GroupBy(d => d.TargetFramework)
                .Select(group => new DependencyGroupItem
                {
                    TargetFramework = group.Key,

                    // A package that supports a target framework but does not have dependencies while on
                    // that target framework is represented by a fake dependency with a null "Id" and "VersionRange".
                    // This fake dependency should not be included in the output.
                    Dependencies = group
                        .Where(d => d.Key != null && d.VersionRange != null)
                        .Select(d => new DependencyItemRef
                        {
                            Id = d.Key,
                            Range = d.VersionRange
                        } as DependencyItem)
                        .ToList()
                })
                .ToList();
        }
    }
}
