using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;

namespace YuGet.Database.SQLite
{
	internal sealed class SQLiteDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "SQLite";

		public string Sign => DatabaseName;

		public void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration _)
		{
			services.AddDbContext<IYuGetDbContext, SQLiteDbContext>(x => {
				x.UseSqlite(options.Database.ConnectionString);
			});
		}
	}
}
