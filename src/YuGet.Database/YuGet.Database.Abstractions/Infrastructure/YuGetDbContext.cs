using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Database.Abstractions.Extensions;
using YuGet.Database.Models;

namespace YuGet.Database.Abstractions.Infrastructure
{
	public abstract class YuGetDbContext<TContext> : DbContext, IYuGetDbContext
		where TContext : DbContext
	{
		protected YuGetDbContext(DbContextOptions<TContext> options) : base(options)
		{
		}

		/// <inheritdoc/>
		public virtual bool SupportsLimitInSubqueries => true;

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

			modelBuilder.SetDefualtTypeConversion<List<string>, string>(a => string.Join(',', a), b => b.Split(new char[] { ',' }).ToList());
		}


		public DbSet<ApiKey> ApiKeys { get; set; }

		public DbSet<Package> Packages { get; set; }

		public DbSet<PackageDependency> PackageDependencies { get; set; }

		public DbSet<PackageTag> PackageTags { get; set; }

		public DbSet<PackageType> PackageTypes { get; set; }

		public DbSet<Tag> Tags { get; set; }

		public DbSet<TargetFramework> TargetFrameworks{ get; set; }

		public DbSet<UserAccount> UserAccounts { get; set; }

		public DbSet<WorkTeam> WorkTeams { get; set; }

		public DbSet<WorkTeamMember> WorkTeamsMember { get; set; }
	}
}
