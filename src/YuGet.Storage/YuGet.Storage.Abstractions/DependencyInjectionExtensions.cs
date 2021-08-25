using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YuGet.Core.Builder;
using YuGet.Storage.Abstractions;

namespace YuGet.Storage
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddYuGetStorageCore(this IServiceCollection services)
		{
			services.TryAddTransient<ISymbolStorageService, SymbolStorageService>();
			services.TryAddTransient<IPackageStorageService, PackageStorageService>();
			return services;
		}

		public static IYuGetOptionBuilder AddStorage<TProvider>(this IYuGetOptionBuilder builder)
			where TProvider : class, IYuGetStorageProvider, new()
		{
			builder.AddModuleProvider<TProvider>(Protocol.Builder.ModuleProviderType.Stroage);
			return builder;
		}

	}
}
