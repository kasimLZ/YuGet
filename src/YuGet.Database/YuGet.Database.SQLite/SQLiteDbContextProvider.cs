using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.Abstractions;
using YuGet.Database.Abstractions.Options;

namespace YuGet.Database.SQLite
{
	internal sealed class SQLiteDbContextProvider : IYuGetDbContextProvider
	{
		public string DatabaseName => "SQLite";

		public void SetupDbContext(IServiceCollection services, YuGetDatabaseOption option)
		{
			services.AddDbContext<IYuGetDbContext, SQLiteDbContext>(x => {
				x.UseSqlite(option.ConnectString);
			});
		}
	}
}
