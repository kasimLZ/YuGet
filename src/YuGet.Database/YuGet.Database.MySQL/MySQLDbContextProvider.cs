using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;
using YuGet.Protocol.Builder;

namespace YuGet.Database.MySQL
{
	internal sealed class MySQLDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "MySQL";

		public ModuleProviderType ModuleType => ModuleProviderType.Database;

		public string Sign => DatabaseName;

		public void SetupModule(IServiceCollection services, YuGetOptions options)
		{
			services.AddDbContext<IYuGetDbContext, MySQLDbContext>(x => {
				x.UseMySql(options.Database.ConnectionString, ServerVersion.AutoDetect(options.Database.ConnectionString));
			});
		}
	}
}
