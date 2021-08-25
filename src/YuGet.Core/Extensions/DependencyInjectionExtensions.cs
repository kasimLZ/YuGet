using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using YuGet.Storage;

namespace YuGet.Core
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddYuGet(this IServiceCollection services)
        {
            services.AddConfiguration();
            services.AddBaGetServices();
            services.AddFallbackServices();

            return services;
        }

        /// <summary>
        /// Configures and validates options.
        /// </summary>
        /// <typeparam name="TOptions">The options type that should be added.</typeparam>
        /// <param name="services">The dependency injection container to add options.</param>
        /// <param name="key">
        /// The configuration key that should be used when configuring the options.
        /// If null, the root configuration will be used to configure the options.
        /// </param>
        /// <returns>The dependency injection container.</returns>
        public static IServiceCollection AddBaGetOptions<TOptions>(
            this IServiceCollection services,
            string key = null)
            where TOptions : class
        {
            services.AddSingleton<IValidateOptions<TOptions>>(new ValidateBaGetOptions<TOptions>(key));
            services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                if (key != null)
                {
                    config = config.GetSection(key);
                }

                return new BindOptions<TOptions>(config);
            });

            return services;
        }                 

        private static void AddConfiguration(this IServiceCollection services)
        {
            services.AddBaGetOptions<YuGetOptions>();
            services.AddBaGetOptions<DatabaseOptions>(nameof(YuGetOptions.Database));
            services.AddBaGetOptions<FileSystemStorageOptions>(nameof(YuGetOptions.Storage));
            services.AddBaGetOptions<MirrorOptions>(nameof(YuGetOptions.Mirror));
            services.AddBaGetOptions<SearchOptions>(nameof(YuGetOptions.Search));
            services.AddBaGetOptions<StorageOptions>(nameof(YuGetOptions.Storage));
        }

        private static void AddBaGetServices(this IServiceCollection services)
        {
            //services.TryAddSingleton<IFrameworkCompatibilityService, FrameworkCompatibilityService>();
            //services.TryAddSingleton<IPackageDownloadsSource, PackageDownloadsJsonSource>();

           // services.TryAddSingleton<NuGetClient>();
            //services.TryAddSingleton<NullSearchIndexer>();
            ////services.TryAddSingleton<NullSearchService>();
           // services.TryAddSingleton<RegistrationBuilder>();
           // services.TryAddSingleton<SystemTime>();
           // services.TryAddSingleton<ValidateStartupOptions>();

           // services.TryAddSingleton(HttpClientFactory);
           // services.TryAddSingleton(NuGetClientFactoryFactory);

            //services.TryAddScoped<DownloadsImporter>();

            services.TryAddTransient<IAuthenticationService, ApiKeyAuthenticationService>();
            services.TryAddTransient<IPackageContentService, DefaultPackageContentService>();
            //services.TryAddTransient<IPackageDeletionService, PackageDeletionService>();
            //services.TryAddTransient<IPackageIndexingService, PackageIndexingService>();
            services.TryAddTransient<IPackageMetadataService, DefaultPackageMetadataService>();
            //services.TryAddTransient<IServiceIndexService, YuGetServiceIndex>();
            //services.TryAddTransient<ISymbolIndexingService, SymbolIndexingService>();

            //services.TryAddTransient<DatabaseSearchService>();
            //services.TryAddTransient<MirrorService>();
            //services.TryAddTransient<MirrorV2Client>();
            //services.TryAddTransient<MirrorV3Client>();
            //services.TryAddTransient<NullMirrorService>();
           // services.TryAddTransient<PackageService>();

            //services.TryAddTransient(IMirrorServiceFactory);
            //services.TryAddTransient(IMirrorNuGetClientFactory);
        }

        private static void AddFallbackServices(this IServiceCollection services)
        {
           // services.TryAddScoped<IContext, NullContext>();

            // BaGet's services have multiple implementations that live side-by-side.
            // The application will choose the implementation using one of two ways:
            //
            // 1. Using the first implementation that was registered in the dependency injection
            //    container. This is the strategy used by applications that embed BaGet.
            // 2. Using "providers". The providers will examine the application's configuration to
            //    determine whether its service implementation is active. Thsi is the strategy used
            //    by the default BaGet application.
            //
            // BaGet has database and search services, but the database services are special
            // in that they may also act as search services. If an application registers the
            // database service first and the search service second, the application should
            // use the search service even though it wasn't registered first. Furthermore,
            // if an application registers a database service without a search service, the
            // database service should be used for search. This effect is achieved by deferring
            // the database search service's registration until the very end.
            services.TryAddTransient<ISearchIndexer>(provider => provider.GetRequiredService<NullSearchIndexer>());
            services.TryAddTransient<ISearchService>(provider => provider.GetRequiredService<DatabaseSearchService>());
        }

        //private static HttpClient HttpClientFactory(IServiceProvider provider)
        //{
        //    var options = provider.GetRequiredService<IOptions<MirrorOptions>>().Value;

        //    var assembly = Assembly.GetEntryAssembly();
        //    var assemblyName = assembly.GetName().Name;
        //    var assemblyVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";

        //    var client = new HttpClient(new HttpClientHandler
        //    {
        //        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        //    });

        //    client.DefaultRequestHeaders.Add("User-Agent", $"{assemblyName}/{assemblyVersion}");
        //    client.Timeout = TimeSpan.FromSeconds(options.PackageDownloadTimeoutSeconds);

        //    return client;
        //}

        //private static NuGetClientFactory NuGetClientFactoryFactory(IServiceProvider provider)
        //{
        //    var httpClient = provider.GetRequiredService<HttpClient>();
        //    var options = provider.GetRequiredService<IOptions<MirrorOptions>>();

        //    return new NuGetClientFactory(
        //        httpClient,
        //        options.Value.PackageSource.ToString());
        //}

        //private static IMirrorService IMirrorServiceFactory(IServiceProvider provider)
        //{
        //    var options = provider.GetRequiredService<IOptionsSnapshot<MirrorOptions>>();
        //    var service = options.Value.Enabled ? typeof(MirrorService) : typeof(NullMirrorService);
            
        //    return (IMirrorService)provider.GetRequiredService(service);
        //}

        //private static IMirrorNuGetClient IMirrorNuGetClientFactory(IServiceProvider provider)
        //{
        //    var options = provider.GetRequiredService<IOptionsSnapshot<MirrorOptions>>();
        //    var service = options.Value.Legacy ? typeof(MirrorV2Client) : typeof(MirrorV3Client);

        //    return (IMirrorNuGetClient)provider.GetRequiredService(service);
        //}
    }
}
