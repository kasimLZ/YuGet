using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace YuGet.Database.Infrastructure.Storage
{
	internal interface IConverterProvider
	{
		ValueConverter Converter { get; }
	}
}
