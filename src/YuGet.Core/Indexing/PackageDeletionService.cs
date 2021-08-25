using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Versioning;
using System;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Storage;

namespace YuGet.Core
{
	public class PackageDeletionService : IPackageDeletionService
    {
        private readonly IPackageService _packages;
        private readonly IPackageStorageService _storage;
        private readonly YuGetOptions _options;
        private readonly ILogger<PackageDeletionService> _logger;

        public PackageDeletionService(
            IPackageService packages,
            IPackageStorageService storage,
            IOptionsSnapshot<YuGetOptions> options,
            ILogger<PackageDeletionService> logger)
        {
            _packages = packages ?? throw new ArgumentNullException(nameof(packages));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> TryDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
        {
            switch (_options.PackageDeletionBehavior)
            {
                case PackageDeletionBehavior.Unlist:
                    return await TryUnlistPackageAsync(id, version, cancellationToken);

                case PackageDeletionBehavior.HardDelete:
                    return await TryHardDeletePackageAsync(id, version, cancellationToken);

                default:
                    throw new InvalidOperationException($"Unknown deletion behavior '{_options.PackageDeletionBehavior}'");
            }
        }

        private async Task<bool> TryUnlistPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unlisting package {PackageId} {PackageVersion}...", id, version);

            if (!await _packages.UnlistPackageAsync(id, version, cancellationToken))
            {
                _logger.LogWarning("Could not find package {PackageId} {PackageVersion}", id, version);

                return false;
            }

            _logger.LogInformation("Unlisted package {PackageId} {PackageVersion}", id, version);

            return true;
        }

        private async Task<bool> TryHardDeletePackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Hard deleting package {PackageId} {PackageVersion} from the database...",
                id,
                version);

            var found = await _packages.HardDeletePackageAsync(id, version, cancellationToken);
            if (!found)
            {
                _logger.LogWarning(
                    "Could not find package {PackageId} {PackageVersion} in the database",
                    id,
                    version);
            }

            // Delete the package from storage. This is necessary even if the package isn't
            // in the database to ensure that the storage is consistent with the database.
            _logger.LogInformation("Hard deleting package {PackageId} {PackageVersion} from storage...",
                id,
                version);

            await _storage.DeleteAsync(id, version, cancellationToken);

            _logger.LogInformation(
                "Hard deleted package {PackageId} {PackageVersion} from storage",
                id,
                version);

            return found;
        }
    }
}
