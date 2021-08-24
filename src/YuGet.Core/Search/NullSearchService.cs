using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Core.Models;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core
{
    /// <summary>
    /// A minimal search service implementation, used for advanced scenarios.
    /// </summary>
    public class NullSearchService : ISearchService
    {
        private static readonly IReadOnlyList<string> EmptyStringList = new List<string>();

        private static readonly AutocompleteResponse EmptyAutocompleteResponse =
            new AutocompleteResponseRef
            {
                TotalHits = 0,
                Data = EmptyStringList,
                Context = AutocompleteContextRef.Default
            };

        private static readonly DependentsResponse EmptyDependentsResponse =
            new DependentsResponseRef()
            {
                TotalHits = 0,
                Data = new List<DependentResult>()
            };

        private static readonly SearchResponse EmptySearchResponse =
            new SearchResponseRef
            {
                TotalHits = 0,
                Data = new List<SearchResult>()
            };

        public async Task<AutocompleteResponse> AutocompleteAsync(AutocompleteRequest request, CancellationToken _)
        {
            return await Task.FromResult(EmptyAutocompleteResponse);
        }

        public async Task<AutocompleteResponse> ListPackageVersionsAsync(VersionsRequest request, CancellationToken _)
        {
            return await Task.FromResult(EmptyAutocompleteResponse);
        }

        public async Task<DependentsResponse> FindDependentsAsync(string packageKey, CancellationToken _)
        {
            return await Task.FromResult(EmptyDependentsResponse);
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken _)
        {
            return await Task.FromResult(EmptySearchResponse);
        }
    }
}
