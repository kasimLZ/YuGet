using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.PostgreSQL;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPostgreSQLYuGetDbContext(this IServiceCollection services)
		{
			return services.AddYuGetDbContextProvider<PostgreSQLDbContextProvider>();
		}
	}
}
