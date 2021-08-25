using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YuGet.Database;
using YuGet.Database.Models;

namespace YuGet.Core
{
    internal class DownloadsImporter : IDownloadsImporter
    {
        private const int BatchSize = 200;

        private readonly IYuGetDbContext _context;
        private readonly IPackageDownloadsSource _downloadsSource;
        private readonly ILogger<DownloadsImporter> _logger;

        public DownloadsImporter(
            IYuGetDbContext context,
            IPackageDownloadsSource downloadsSource,
            ILogger<DownloadsImporter> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _downloadsSource = downloadsSource ?? throw new ArgumentNullException(nameof(downloadsSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ImportAsync(CancellationToken cancellationToken)
        {
            var packageDownloads = await _downloadsSource.GetPackageDownloadsAsync();
            var packages = await _context.Set<Package>().CountAsync();
            var batches = (packages / BatchSize) + 1;

            for (var batch = 0; batch < batches; batch++)
            {
                _logger.LogInformation("Importing batch {Batch}...", batch);

                foreach (var package in await GetBatchAsync(batch, cancellationToken))
                {
                    var packageKey = package.Key.ToLowerInvariant();
                    var packageVersion = package.NormalizedVersionString.ToLowerInvariant();

                    if (!packageDownloads.ContainsKey(packageKey) ||
                        !packageDownloads[packageKey].ContainsKey(packageVersion))
                    {
                        continue;
                    }

                    package.Downloads = packageDownloads[packageKey][packageVersion];
                }

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Imported batch {Batch}", batch);
            }
        }

        private Task<List<Package>> GetBatchAsync(int batch, CancellationToken cancellationToken)
            => _context.Set<Package>()
                .OrderBy(p => p.Key)
                .Skip(batch * BatchSize)
                .Take(BatchSize)
                .ToListAsync(cancellationToken);
    }
}
