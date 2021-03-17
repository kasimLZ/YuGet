using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YuGet.Database.Infrastructure.Storage
{
	internal static class ConverterProviderExtension
	{
		private static Dictionary<Type ,ValueConverter> TypeConverterCollection = new Dictionary<Type, ValueConverter>();

		internal static ModelBuilder UseDefualtTypeConvter<TModel, TProvider>(this ModelBuilder builder)
		{
			if (TypeConverterCollection.ContainsKey(typeof(TModel)))
				throw new ArgumentException($"The current type {typeof(TModel).FullName} is already set");

			TypeConverterCollection.Add(typeof(TModel), new CastingConverter<TModel, TProvider>());

			return builder;
		}

		internal static ModelBuilder UseConvterProvider(this ModelBuilder builder)
		{
			var mutableEntityTypes = builder.Model.GetEntityTypes();

			foreach (var entityType in mutableEntityTypes)
			{
				foreach  (var entityProperty in entityType.ClrType.GetProperties())
				{
					if (entityProperty.GetCustomAttributes(false)
							.FirstOrDefault(a => typeof(IConverterProvider).IsAssignableFrom(a.GetType())) is IConverterProvider Provider)
						builder.Entity(entityType.Name).Property(entityProperty.Name).HasConversion(Provider.Converter);

					if (TypeConverterCollection.ContainsKey(entityProperty.PropertyType))
					{
						builder.Entity(entityType.Name).Property(entityProperty.Name).HasConversion(TypeConverterCollection[entityProperty.PropertyType]);
					}
				}
			}

			return builder;
		}
	}
}
