using Microsoft.Extensions.Configuration;
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
using YuGet.Database;
using YuGet.Storage;

namespace YuGet
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddYuget(this IServiceCollection services, Action<IYuGetOptionBuilder> optionBuilder)
		{
			
			using var provider = services.BuildServiceProvider();

			var builder = new YuGetOptionBuilder
			{
				Service = services,
				Options = new(),
				Configuration = provider.GetRequiredService<IConfiguration>()
			};

			builder.Configuration.Bind(builder.Options);

			builder.Service.Configure<YuGetOptions>(builder.Configuration);

			builder.Service.TryAddSingleton<IFrameworkCompatibilityService, FrameworkCompatibilityService>();

			builder.Service.TryAddSingleton<NuGetClient>();
			builder.Service.TryAddSingleton<RegistrationBuilder>();
			builder.Service.TryAddSingleton<SystemTime>();
			builder.Service.TryAddSingleton<ValidateStartupOptions>();

			builder.Service.TryAddTransient<PackageService>();

			builder.Service.AddHttpClientService()
				.AddMirrorService()
				.AddNullSearchService()
				.AddIndexingService()
				.AddNuGetClientFactory()
				.AddYuGetDbContextCore()
				.AddYuGetStorageCore();

			builder.Service.TryAddTransient<DatabaseSearchService>();

			builder.Service.TryAddTransient<IPackageContentService, DefaultPackageContentService>();
			builder.Service.TryAddTransient<IPackageMetadataService, DefaultPackageMetadataService>();

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
