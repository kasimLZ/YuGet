using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Core.Models;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Internal
{
    public class NullCatalogClient : ICatalogClient
    {
        public async Task<CatalogIndex> GetIndexAsync(CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new CatalogIndexRef
            {
                CommitTimestamp = DateTimeOffset.MinValue,
                Count = 0,
                Items = new List<CatalogPageItem>()
            });
        }

        public Task<CatalogPage> GetPageAsync(string pageUrl, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException($"{nameof(NullCatalogClient)} does not support loading catalog pages.");
        }

        public Task<PackageDeleteCatalogLeaf> GetPackageDeleteLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException($"{nameof(NullCatalogClient)} does not support loading catalog leaves.");
        }

        public Task<PackageDetailsCatalogLeaf> GetPackageDetailsLeafAsync(string leafUrl, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException($"{nameof(NullCatalogClient)} does not support loading catalog leaves.");
        }
    }
}
