using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.SQLServer;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSQLServerYuGetDbContext(this IServiceCollection services)
		{
			return services.AddYuGetDbContextProvider<SQLServerDbContextProvider>();
		}
	}
}
