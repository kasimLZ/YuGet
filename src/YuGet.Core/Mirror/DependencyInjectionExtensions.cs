using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace YuGet.Core.Mirror
{
	internal static class DependencyInjectionExtensions
	{
		internal static IServiceCollection AddMirrorService(this IServiceCollection services)
		{
            services.TryAddSingleton<IPackageDownloadsSource, PackageDownloadsJsonSource>();

            services.TryAddScoped<DownloadsImporter>();

            services.TryAddTransient<MirrorV2Client>();
			services.TryAddTransient<MirrorV3Client>();

			services.TryAddTransient<MirrorService>();
			services.TryAddTransient<NullMirrorService>();

            services.TryAddTransient(IMirrorServiceFactory);
            services.TryAddTransient(IMirrorNuGetClientFactory);

            return services;
        }


        private static IMirrorService IMirrorServiceFactory(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptionsSnapshot<MirrorOptions>>();
            var service = options.Value.Enabled ? typeof(MirrorService) : typeof(NullMirrorService);

            return (IMirrorService)provider.GetRequiredService(service);
        }

        private static IMirrorNuGetClient IMirrorNuGetClientFactory(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptionsSnapshot<MirrorOptions>>();
            var service = options.Value.Legacy ? typeof(MirrorV2Client) : typeof(MirrorV3Client);

            return (IMirrorNuGetClient)provider.GetRequiredService(service);
        }
    }
}
