using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YuGet.Database;
using YuGet.Database.Abstractions.Infrastructure;
using YuGet.Database.Abstractions.Options;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddYuGetDbCore(this IServiceCollection services) 
		{
			using var provider =  services.BuildServiceProvider();

			var configuraion = provider.GetRequiredService<IConfiguration>();

			services.Configure<YuGetDatabaseOption>(configuraion.GetSection(YuGetDatabaseOption.SectionName));

			services.TryAddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));

			return services;
		}
	}
}
