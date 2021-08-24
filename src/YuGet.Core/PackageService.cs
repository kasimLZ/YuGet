using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Database;
using YuGet.Database.Models;

namespace YuGet.Core
{
	public class PackageService : IPackageService
    {
        private readonly IYuGetDbContext _context;

        public PackageService(IYuGetDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PackageAddResult> AddAsync(Package package, CancellationToken cancellationToken)
        {
            try
            {
                _context.Set<Package>().Add(package);

                await _context.SaveChangesAsync(cancellationToken);

                return PackageAddResult.Success;
            }
            catch (DbUpdateException e)
                when (_context.IsUniqueConstraintViolationException(e))
            {
                return PackageAddResult.PackageAlreadyExists;
            }
        }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken)
        {
            return await _context
                .Set<Package>()
                .Where(p => p.Key == key)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(string key, NuGetVersion version, CancellationToken cancellationToken)
        {
            return await _context
                .Set<Package>()
                .Where(p => p.Key == key)
                .Where(p => p.NormalizedVersionString == version.ToNormalizedString())
                .AnyAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Package>> FindAsync(string key, bool includeUnlisted, CancellationToken cancellationToken)
        {
            var query = _context.Set<Package>()
                .Include(p => p.Dependencies)
                .Include(p => p.PackageTypes)
                .Include(p => p.TargetFrameworks)
                .Where(p => p.Key == key);

            if (!includeUnlisted)
            {
                query = query.Where(p => p.Listed);
            }

            return (await query.ToListAsync(cancellationToken)).AsReadOnly();
        }

        public Task<Package> FindOrNullAsync(
            string key,
            NuGetVersion version,
            bool includeUnlisted,
            CancellationToken cancellationToken)
        {
            var query = _context.Set<Package>()
                .Include(p => p.Dependencies)
                .Include(p => p.TargetFrameworks)
                .Where(p => p.Key == key)
                .Where(p => p.NormalizedVersionString == version.ToNormalizedString());

            if (!includeUnlisted)
            {
                query = query.Where(p => p.Listed);
            }

            return query.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<bool> UnlistPackageAsync(string key, NuGetVersion version, CancellationToken cancellationToken)
        {
            return TryUpdatePackageAsync(key, version, p => p.Listed = false, cancellationToken);
        }

        public Task<bool> RelistPackageAsync(string key, NuGetVersion version, CancellationToken cancellationToken)
        {
            return TryUpdatePackageAsync(key, version, p => p.Listed = true, cancellationToken);
        }

        public Task<bool> AddDownloadAsync(string key, NuGetVersion version, CancellationToken cancellationToken)
        {
            return TryUpdatePackageAsync(key, version, p => p.Downloads += 1, cancellationToken);
        }

        public async Task<bool> HardDeletePackageAsync(string key, NuGetVersion version, CancellationToken cancellationToken)
        {
            var package = await _context.Set<Package>()
                .Where(p => p.Key == key)
                .Where(p => p.NormalizedVersionString == version.ToNormalizedString())
                .Include(p => p.Dependencies)
                .Include(p => p.TargetFrameworks)
                .FirstOrDefaultAsync(cancellationToken);

            if (package == null)
            {
                return false;
            }

            _context.Set<Package>().Remove(package);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task<bool> TryUpdatePackageAsync(
            string key,
            NuGetVersion version,
            Action<Package> action,
            CancellationToken cancellationToken)
        {
            var package = await _context.Set<Package>()
                .Where(p => p.Key == key)
                .Where(p => p.NormalizedVersionString == version.ToNormalizedString())
                .FirstOrDefaultAsync();

            if (package != null)
            {
                action(package);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }

            return false;
        }
    }
}
