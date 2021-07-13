using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace YuGet.Database
{
	public interface IYuGetDbContext
	{
        DatabaseFacade Database { get; }

        /// <inheritdoc cref="DbContext.Set{TEntity}"/>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <inheritdoc cref="DbContext.Entry(object)"/>
        EntityEntry Entry([NotNull] object entity);

        /// <inheritdoc cref="DbContext.SaveChanges"/>
        int SaveChanges();

        /// <inheritdoc cref="DbContext.SaveChanges(bool)"/>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <inheritdoc cref="DbContext.SaveChangesAsync(bool, CancellationToken)"/>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check whether a <see cref="DbUpdateException"/> is due to a SQL unique constraint violation.
        /// </summary>
        /// <param name="exception">The exception to inspect.</param>
        /// <returns>Whether the exception was caused to SQL unique constraint violation.</returns>
        bool IsUniqueConstraintViolationException(DbUpdateException exception);

        /// <summary>
        /// Whether this database engine supports LINQ "Take" in subqueries.
        /// </summary>
        bool SupportsLimitInSubqueries { get; }

        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Creates the database if it does not already exist.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the task.</param>
        /// <returns>A task that completes once migrations are applied.</returns>
        Task RunMigrationsAsync(CancellationToken cancellationToken);
    }
}
