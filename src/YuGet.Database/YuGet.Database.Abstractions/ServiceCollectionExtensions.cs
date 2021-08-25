using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YuGet.Core.Builder;
using YuGet.Database;
using YuGet.Database.Abstractions;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddYuGetDbContextCore(this IServiceCollection services) 
		{
			services.TryAddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
			return services;
		}

		public static IYuGetOptionBuilder AddDatabase<TProvider>(this IYuGetOptionBuilder builder)
			where TProvider : class, IYuGetDbContextProvider, new()
		{
			return builder.AddModuleProvider<IYuGetDbContextProvider, TProvider>();
		}
	}
}
