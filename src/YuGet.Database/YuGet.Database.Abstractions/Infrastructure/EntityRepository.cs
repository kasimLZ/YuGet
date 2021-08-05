using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace YuGet.Database.Abstractions.Infrastructure
{
	internal class EntityRepository<TEntity> : IEntityRepository<TEntity>
		where TEntity : EntitySet
	{
		protected readonly IYuGetDbContext context;
		protected readonly DbSet<TEntity> dbset;


		public EntityRepository(IYuGetDbContext context)
		{
			this.context = context;
			dbset = context.Set<TEntity>();
		}

		/// <inheritdoc/>
		public virtual void Add(TEntity entity)
		{
			entity.Id = Guid.NewGuid();
			dbset.Add(entity);
		}

		/// <inheritdoc/>
		public TEntity GetById(Guid Id) => dbset.Find(new object[] { Id });

		/// <inheritdoc/>
		public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression) => dbset.Where(expression ?? (a => true));

		/// <inheritdoc/>
		public void Remove(Guid Id) => Remove(GetById(Id));

		/// <inheritdoc/>
		public void Remove(TEntity item) => dbset.Remove(item);

		/// <inheritdoc/>
		public void Remove(Expression<Func<TEntity, bool>> where) => Query(where).ToList().ForEach(a => Remove(a));

		/// <inheritdoc/>
		public void Save(Guid? Id, TEntity entity)
		{
			if (!Id.HasValue)
			{
				Add(entity);
				return;
			}
			Update(entity);
		}

		/// <inheritdoc/>
		public void Update(TEntity entity)
		{
			dbset.Attach(entity);

			var entityEntry = context.Entry(entity);

			entityEntry.State = EntityState.Modified;

			foreach (var prop in typeof(TEntity).GetProperties().Where(a => a.GetCustomAttributes(typeof(NotModifiedAttribute), false).Any()))
				if (entityEntry.Property(prop.Name) != null)
					entityEntry.Property(prop.Name).IsModified = false;
		}

		public int Commit() => context.SaveChanges();


		public Task<int> CommitAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
	}
}
