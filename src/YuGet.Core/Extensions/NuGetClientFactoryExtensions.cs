using YuGet.Core.Catalog;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace YuGet.Core
{
    internal static class NuGetClientFactoryExtensions
    {
        public static IServiceCollection AddNuGetClientFactory(this IServiceCollection services)
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

        /// <summary>
        /// Create a new <see cref="CatalogProcessor"/> to discover and download catalog leafs.
        /// Leafs are processed by the <see cref="ICatalogLeafProcessor"/>.
        /// </summary>
        /// <param name="clientFactory">The factory used to create NuGet clients.</param>
        /// <param name="cursor">Cursor to track succesfully processed leafs. Leafs before the cursor are skipped.</param>
        /// <param name="leafProcessor">The leaf processor.</param>
        /// <param name="options">The options to configure catalog processing.</param>
        /// <param name="logger">The logger used for telemetry.</param>
        /// <returns>The catalog processor.</returns>
        public static CatalogProcessor CreateCatalogProcessor(
            this NuGetClientFactory clientFactory,
            ICursor cursor,
            ICatalogLeafProcessor leafProcessor,
            CatalogProcessorOptions options,
            ILogger<CatalogProcessor> logger)
        {
            var catalogClient = clientFactory.CreateCatalogClient();

            return new CatalogProcessor(
                cursor,
                catalogClient,
                leafProcessor,
                options,
                logger);
        }
    }
}
