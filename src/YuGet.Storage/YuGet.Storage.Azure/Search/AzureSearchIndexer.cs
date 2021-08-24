using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using YuGet.Database.Models;
using YuGet.Core;

namespace YuGet.Storage.Azure
{
    public class AzureSearchIndexer : ISearchIndexer
    {
        private readonly IPackageService _packages;
        private readonly IndexActionBuilder _actionBuilder;
        private readonly AzureSearchBatchIndexer _batchIndexer;
        private readonly ILogger<AzureSearchIndexer> _logger;

        public AzureSearchIndexer(
            IPackageService packages,
            IndexActionBuilder actionBuilder,
            AzureSearchBatchIndexer batchIndexer,
            ILogger<AzureSearchIndexer> logger)
        {
            _packages = packages ?? throw new ArgumentNullException(nameof(packages));
            _actionBuilder = actionBuilder ?? throw new ArgumentNullException(nameof(actionBuilder));
            _batchIndexer = batchIndexer ?? throw new ArgumentNullException(nameof(batchIndexer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task IndexAsync(Package package, CancellationToken cancellationToken)
        {
            var packages = await _packages.FindAsync(package.Key, includeUnlisted: false, cancellationToken);

            var actions = _actionBuilder.UpdatePackage(new PackageRegistration(package.Key, packages));

            await _batchIndexer.IndexAsync(actions, cancellationToken);
        }
    }
}
