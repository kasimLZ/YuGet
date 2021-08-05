using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.Abstractions;
using YuGet.Database.Abstractions.Options;

namespace YuGet.Database.MySQL
{
	internal sealed class MySQLDbContextProvider : IYuGetDbContextProvider
	{
		public string DatabaseName => "MySQL";

		public void SetupDbContext(IServiceCollection services, YuGetDatabaseOption option)
		{
			services.AddDbContext<IYuGetDbContext, MySQLDbContext>(x => {
				x.UseMySql(option.ConnectString, ServerVersion.AutoDetect(option.ConnectString));
			});
		}
	}
}
