using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace YuGet.Database.Infrastructure.Storage
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class ConvertCastingAttribute : Attribute, IConverterProvider
	{
		private static readonly object[] DefualtConstructorParameters = new object[] { null };

		internal ConvertCastingAttribute(Type ModelType, Type ProviderType)
		{ 
			Converter = Activator.CreateInstance(typeof(CastingConverter<,>).MakeGenericType(ModelType, ProviderType), DefualtConstructorParameters) as ValueConverter;
		}

		public ValueConverter Converter { get; private set; }
	}
}
