using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using YuGet.Core;
using YuGet.Core.Builder;
using YuGet.Core.Indexing;
using YuGet.Core.Mirror;
using YuGet.Core.Search;

namespace YuGet
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddYuget(this IServiceCollection services, Action<IYuGetOptionBuilder> optionBuilder)
		{
			services.TryAddSingleton<IFrameworkCompatibilityService, FrameworkCompatibilityService>();

			services.TryAddSingleton<NuGetClient>();
			services.TryAddSingleton<RegistrationBuilder>();
			services.TryAddSingleton<SystemTime>();
			services.TryAddSingleton<ValidateStartupOptions>();

			services.TryAddTransient<PackageService>();

			services.AddHttpClientService()
				.AddMirrorService()
				.AddNullSearchService()
				.AddIndexingService()
				.AddNuGetClientFactory()
				.AddYuGetDbContextCore()
				.AddYuGetStorageCore();

			services.TryAddTransient<DatabaseSearchService>();

			services.TryAddTransient<IPackageContentService, DefaultPackageContentService>();
			services.TryAddTransient<IPackageMetadataService, DefaultPackageMetadataService>();

			var builder = new YuGetOptionBuilder
			{
				Service = services
			};

			optionBuilder.Invoke(builder);

			return services;
		}

		private static IServiceCollection AddHttpClientService(this IServiceCollection services)
		{

			services.TryAddSingleton(provider => {

				var options = provider.GetRequiredService<IOptions<MirrorOptions>>().Value;

				var client = new HttpClient(new HttpClientHandler
				{
					AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				});

				client.DefaultRequestHeaders.Add("User-Agent", "Dotnet 5.0/yuget-1.0.0");
				client.Timeout = TimeSpan.FromSeconds(options.PackageDownloadTimeoutSeconds);

				return client;
			});


			return services;
		}

		private static IServiceCollection AddNuGetClientFactory(this IServiceCollection services)
		{
			services.TryAddSingleton(provider => {
				var httpClient = provider.GetRequiredService<HttpClient>();
				var options = provider.GetRequiredService<IOptions<MirrorOptions>>();

				return new NuGetClientFactory(
					httpClient,
					options.Value.PackageSource.ToString());
			});

			return services;
		}
	}
}
