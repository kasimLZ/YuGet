using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace YuGet.Database.Infrastructure.Storage
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class ConvertBoolToZeroOneAttribute : Attribute, IConverterProvider
	{
		internal ConvertBoolToZeroOneAttribute(Type ProviderType) 
		{
			Converter = Activator.CreateInstance(typeof(BoolToZeroOneConverter<>).MakeGenericType(ProviderType)) as ValueConverter;
		}

		public ValueConverter Converter { get; private set; }
	}
}
