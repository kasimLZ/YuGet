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

		public void RegistModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration)
		{
			services.AddDbContext<IYuGetDbContext, SQLiteDbContext>(x => {
				x.UseSqlite(options.Database.ConnectionString);
			});
		}
	}
}
