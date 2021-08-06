using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace YuGet.Database.Abstractions.Extensions
{
	internal static class TypeComparerExtensions
	{
		public static void SetDefualtTypeComparer<T>(this ModelBuilder builder, Expression<Func<T, T, bool>> equals, Expression<Func<T, int>> hashCode)
		{
			var converter = new DefulatValueComparer<T>(equals, hashCode);
			builder.SetDefualtTypeComparer(converter);
		}

		public static void SetDefualtTypeComparer<T>(this ModelBuilder builder, Expression<Func<T, T, bool>> equals, Expression<Func<T, int>> hashCode, Expression<Func<T, T>> snapshot)
		{
			var converter = new DefulatValueComparer<T>(equals, hashCode, snapshot);
			builder.SetDefualtTypeComparer(converter);
		}

		public static void SetDefualtTypeComparer<T>(this ModelBuilder builder, ValueComparer<T> comparer)
		{
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(T));
				foreach (var property in properties)
				{
					builder.Entity(entityType.Name).Property(property.Name).Metadata.SetValueComparer(comparer);
				}
			}
		}

		private class DefulatValueComparer<T> : ValueComparer<T>
		{
			public DefulatValueComparer(Expression<Func<T, T, bool>> equals, Expression<Func<T, int>> hashCode) : base(equals, hashCode) { }

			public DefulatValueComparer(Expression<Func<T, T, bool>> equals, Expression<Func<T, int>> hashCode, Expression<Func<T, T>> snapshot) : base(equals, hashCode, snapshot) { }
		}
	}
}
