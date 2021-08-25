using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;
using YuGet.Protocol.Builder;

namespace YuGet.Database.SQLServer
{
	internal sealed class SQLServerDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "Mssql";

		public ModuleProviderType ModuleType => ModuleProviderType.Database;

		public string Sign => DatabaseName;

		public void SetupModule(IServiceCollection services, YuGetOptions options)
		{
			services.AddDbContext<IYuGetDbContext, SQLServerDbContext>(x => {
				x.UseSqlServer(options.Database.ConnectionString);
			});
		}
	}
}
