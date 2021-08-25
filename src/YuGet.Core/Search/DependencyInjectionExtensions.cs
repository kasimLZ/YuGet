using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace YuGet.Core.Search
{
	internal static class DependencyInjectionExtensions
	{
		internal static IServiceCollection AddNullSearchService(this IServiceCollection services)
		{
			services.TryAddSingleton<NullSearchIndexer>();
			services.TryAddSingleton<NullSearchService>();

			return services;
		}
	}
}
