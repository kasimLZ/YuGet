using YuGet.Core.Builder;
using YuGet.Database.SQLite;

namespace YuGet.Database
{
	public static class ServiceCollectionExtensions
	{
		public static IYuGetOptionBuilder AddSQLite(this IYuGetOptionBuilder builder) => builder.AddDatabase<SQLiteDbContextProvider>();
	}
}
