using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database
{
	public interface IEntityRepository<TEntity> where TEntity : EntitySet
	{
		/// <summary>
		/// Insert data
		/// </summary>
		/// <param name="entity">Record entity</param>
		void Add(TEntity entity);

		/// <summary>
		/// Physical delete record
		/// </summary>
		/// <param name="key">ID</param>
		void Remove(Snid key);

		/// <summary>
		/// Physical delete record
		/// </summary>
		/// <param name="item">Record entity</param>
		void Remove(TEntity item);

		/// <summary>
		/// Physical delete record
		/// </summary>
		/// <param name="where">Filter condition expression</param>
		void Remove(Expression<Func<TEntity, bool>> where);

		/// <summary>
		/// Retrieve data based on filter criteria
		/// </summary>
		/// <param name="where">Filter condition expression</param>
		/// <returns>Data query object</returns>
		IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);

		/// <summary>
		/// Query data based on index
		/// </summary>
		/// <param name="key">Index</param>
		/// <returns>Entity</returns>
		TEntity GetById(Snid key);

		/// <summary>
		/// Save general method
		/// </summary>
		/// <param name="key">Index, Nullable, When it is empty, it is saved. When there is value, it is modified.</param>
		/// <param name="entity">Record</param>
		void Save(Snid? key, TEntity entity);

		/// <summary>
		/// Update data
		/// </summary>
		/// <param name="entity">Record</param>
		void Update(TEntity entity);

		/// <inheritdoc cref="Microsoft.EntityFrameworkCore.DbContext.SaveChanges" />
		int Commit();

		/// <inheritdoc cref="Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(CancellationToken)" />
		Task<int> CommitAsync(CancellationToken cancellationToken = default);
	}
}
