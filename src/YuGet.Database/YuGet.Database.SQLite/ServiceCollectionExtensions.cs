using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.SQLite;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSQLiteYuGetDbContext(this IServiceCollection services)
		{
			return services.AddYuGetDbContextProvider<SQLiteDbContextProvider>();
		}
	}
}
