using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using YuGet.Core;
using YuGet.Core.Builder;
using YuGet.Core.Indexing;
using YuGet.Core.Mirror;
using YuGet.Core.Search;
using YuGet.Database;
using YuGet.Protocol.Builder;
using YuGet.Storage;

namespace YuGet
{
	public static class DependencyInjectionExtensions
	{
		public static IServiceCollection AddYuget(this IServiceCollection services)
		{
			return services.AddYugetCore(builder => {
				// Inject Database Provider
				builder.AddSQLite();

				// Inject Stroage Provider
				builder.AddFileSystem();
			});
		}

		public static IServiceCollection AddYugetCore(this IServiceCollection services, Action<IYuGetOptionBuilder> optionBuilder = null)
		{
			var builder = new YuGetOptionBuilder
			{
				Service = services,
				Options = new()
			};

			using (var provider = services.BuildServiceProvider()) 
			{
				builder.Configuration = provider.GetRequiredService<IConfiguration>();
			}

			if (optionBuilder != null)
			{
				optionBuilder.Invoke(builder);
			}

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

			builder.Service.AddSingleton(builder);

			

			return services;
		}


		public static IApplicationBuilder UseYuGet(this IApplicationBuilder app)
		{
			var builder = app.ApplicationServices.GetService<YuGetOptionBuilder>();

			foreach(var module in Enum.GetValues<ModuleProviderType>())
			{
				var provider = builder[module, builder.Options.Database.Type];
			}

			return builder;
		}
	}
}
