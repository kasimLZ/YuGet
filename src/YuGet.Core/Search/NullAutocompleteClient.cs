using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Core.Models;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Internal
{
    public class NullAutocompleteClient : IAutocompleteClient
    {
        public async Task<AutocompleteResponse> AutocompleteAsync(string query = null, int skip = 0, int take = 20, bool includePrerelease = true, bool includeSemVer2 = true, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new AutocompleteResponseRef
            {
                TotalHits = 0,
                Data = new List<string>()
            });
        }

        public async Task<AutocompleteResponse> ListPackageVersionsAsync(string packageId, bool includePrerelease = true, bool includeSemVer2 = true, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new AutocompleteResponseRef
            {
                TotalHits = 0,
                Data = new List<string>()
            });
        }
    }
}
