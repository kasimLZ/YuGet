using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Database.Abstractions.Extensions;

namespace YuGet.Database.Abstractions.Infrastructure
{
	public abstract class YuGetDbContext<TContext> : DbContext, IYuGetDbContext
		where TContext : DbContext
	{
		/// <inheritdoc/>
		public bool SupportsLimitInSubqueries => throw new NotImplementedException();

		/// <inheritdoc/>
		public abstract bool IsUniqueConstraintViolationException(DbUpdateException exception);

		/// <inheritdoc/>
		public virtual async Task RunMigrationsAsync(CancellationToken cancellationToken)
			=> await Database.MigrateAsync(cancellationToken);

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.SetDefualtTypeConversion<Snid, ulong>(a => a, b => b);

			modelBuilder.SetDefualtTypeConversion<Uri, string>(a => a.AbsoluteUri, b => new Uri(b));
		}

	}
}
