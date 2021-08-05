using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.MySQL;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddMySQLYuGetDbContext(this IServiceCollection services)
		{
			return services.AddYuGetDbContextProvider<MySQLDbContextProvider>();
		}
	}
}
