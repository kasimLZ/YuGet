using YuGet.Core.Builder;
using YuGet.Database.SQLServer;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IYuGetOptionBuilder AddSQLServer(this IYuGetOptionBuilder builder) => builder.AddDatabase<SQLServerDbContextProvider>();
	}
}
