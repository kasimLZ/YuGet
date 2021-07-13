using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace YuGet.Database.Abstractions.Extensions
{
	internal static class TypeConversionExtensions
	{
		public static void SetDefualtTypeConversion<TModel, TProvider>(this ModelBuilder builder, Expression<Func<TModel, TProvider>> To, Expression<Func<TProvider, TModel>> From)
		{
			var converter = new ValueConverter<TModel, TProvider>(To, From);
			builder.SetDefualtTypeConversion(converter);
		}

		public static void SetDefualtTypeConversion<TModel, TProvider>(this ModelBuilder builder, ValueConverter<TModel, TProvider> converter)
		{
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(TModel));
				foreach (var property in properties)
				{
					builder.Entity(entityType.Name).Property(property.Name)
						.HasConversion(converter);
				}
			}
		}
	}
}
