using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace YuGet.Database.Infrastructure.Storage
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class ConvertAnyProvider : Attribute, IConverterProvider
	{
		internal ConvertAnyProvider(ValueConverter Converter) => this.Converter = Converter;

		public ValueConverter Converter { get; set; }
	}
}
