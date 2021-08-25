using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace YuGet.Core.Indexing
{
	internal static class DependencyInjectionExtensions
	{
		internal static IServiceCollection AddIndexingService(this IServiceCollection services)
		{
			services.TryAddTransient<IPackageDeletionService, PackageDeletionService>();
			services.TryAddTransient<IPackageIndexingService, PackageIndexingService>();
			services.TryAddTransient<IServiceIndexService, YuGetServiceIndex>();
			services.TryAddTransient<ISymbolIndexingService, SymbolIndexingService>();
			return services;
		}
	}
}
