using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Database.Abstractions.Infrastructure;
using YuGet.Database.Models;

namespace YuGet.Database.PostgreSQL
{
	internal sealed class PostgreSQLDbContext : YuGetDbContext<PostgreSQLDbContext>, IYuGetDbContext
	{
        /// <summary>
        /// The PostgreSql error code for when a unique constraint is violated.
        /// See: https://www.postgresql.org/docs/9.6/errcodes-appendix.html
        /// </summary>
        private const int UniqueConstraintViolationErrorCode = 23505;

        public PostgreSQLDbContext(DbContextOptions<PostgreSQLDbContext> options) : base(options) { }

		public override bool IsUniqueConstraintViolationException(DbUpdateException exception)
		{
            return exception.InnerException is PostgresException postgresException &&
                  int.TryParse(postgresException.SqlState, out var code) &&
                  code == UniqueConstraintViolationErrorCode;
        }

        public override async Task RunMigrationsAsync(CancellationToken cancellationToken)
        {
            await base.RunMigrationsAsync(cancellationToken);

            // Npgsql caches the database's type information on the initial connection.
            // This causes issues when BaGet creates the database as it may add the citext
            // extension to support case insensitive columns.
            // See: https://github.com/loic-sharma/BaGet/issues/442
            // See: https://github.com/npgsql/efcore.pg/issues/170#issuecomment-303417225
            if (Database.GetDbConnection() is NpgsqlConnection connection)
            {
                await connection.OpenAsync(cancellationToken);
                connection.ReloadTypes();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("citext");

            builder.Entity<Package>()
                .Property(p => p.Key)
                .HasColumnType("citext");

            builder.Entity<Package>()
                .Property(p => p.NormalizedVersionString)
                .HasColumnType("citext");

            builder.Entity<PackageDependency>()
                .Property(p => p.Key)
                .HasColumnType("citext");

            builder.Entity<PackageType>()
                .Property(p => p.Name)
                .HasColumnType("citext");

            builder.Entity<TargetFramework>()
                .Property(p => p.Moniker)
                .HasColumnType("citext");
        }
    }
}
