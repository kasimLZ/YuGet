using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;
using YuGet.Protocol.Builder;

namespace YuGet.Database.SQLite
{
	internal sealed class SQLiteDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "SQLite";

		public ModuleProviderType ModuleType => ModuleProviderType.Database;

		public string Sign => DatabaseName;

		public void SetupModule(IServiceCollection services, YuGetOptions options)
		{
			services.AddDbContext<IYuGetDbContext, SQLiteDbContext>(x => {
				x.UseSqlite(options.Database.ConnectionString);
			});
		}
	}
}
