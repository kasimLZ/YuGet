using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YuGet.Database.Abstractions;
using YuGet.Storage;
using YuGet.Storage.Abstractions;

namespace YuGet
{
	public static class DependencyInjectionExtensions
	{
		private static readonly Dictionary<Type, IYuGetStorageProvider> storageProviders = new();

		public static IServiceCollection AddYuGetStorageCore(this IServiceCollection services)
		{
			services.AddTransient<ISymbolStorageService, SymbolStorageService>();
			services.AddTransient<IPackageStorageService, PackageStorageService>();

			return services;
		}

		public static IServiceCollection AddYuGetStorage(this IServiceCollection services)
		{
			services.AddYuGetDbContextCore();

			//storageProviders.Values.

			return services;
		}

		public static IServiceCollection AddYuGetStorageProvider<TProvider>(this IServiceCollection services)
			where TProvider : class, IYuGetStorageProvider, new()
		{
			storageProviders.TryAdd(typeof(TProvider), new TProvider());
			return services;
		}
	}
}
