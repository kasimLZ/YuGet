using YuGet.Core.Builder;
using YuGet.Database.MySQL;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		public static IYuGetOptionBuilder AddMySQL(this IYuGetOptionBuilder builder) => builder.AddDatabase<MySQLDbContextProvider>();
	}
}
