using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YuGet.Database.Abstractions.Infrastructure;
using YuGet.Database.Models;

namespace YuGet.Database.SQLite
{
	internal sealed class SQLiteDbContext : YuGetDbContext<SQLiteDbContext>, IYuGetDbContext
	{
		/// <summary>
		/// The Sqlite error code for when a unique constraint is violated.
		/// </summary>
		private const int SqliteUniqueConstraintViolationErrorCode = 19;

		public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options) : base(options) { }

		public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
		{
			return exception.InnerException is SqliteException sqliteException &&
				sqliteException.SqliteErrorCode == SqliteUniqueConstraintViolationErrorCode;
		}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Package>()
                .Property(p => p.Key)
                .HasColumnType("TEXT COLLATE NOCASE");

            builder.Entity<Package>()
                .Property(p => p.NormalizedVersionString)
                .HasColumnType("TEXT COLLATE NOCASE");

            builder.Entity<PackageDependency>()
                .Property(d => d.Key)
                .HasColumnType("TEXT COLLATE NOCASE");

            builder.Entity<PackageType>()
                .Property(t => t.Name)
                .HasColumnType("TEXT COLLATE NOCASE");

            builder.Entity<TargetFramework>()
                .Property(f => f.Moniker)
                .HasColumnType("TEXT COLLATE NOCASE");
        }
    }
}
