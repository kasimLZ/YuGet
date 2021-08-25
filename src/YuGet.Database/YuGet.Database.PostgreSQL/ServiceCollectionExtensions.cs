using YuGet.Core.Builder;
using YuGet.Database.PostgreSQL;

namespace YuGet.Database
{
	public static class ServiceCollectionExtensions
	{
		public static IYuGetOptionBuilder AddPostgreSQL(this IYuGetOptionBuilder services) => services.AddDatabase<PostgreSQLDbContextProvider>();
	}
}
