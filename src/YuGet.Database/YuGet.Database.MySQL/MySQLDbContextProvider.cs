using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;

namespace YuGet.Database.MySQL
{
	internal sealed class MySQLDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "MySQL";

		public string Sign => DatabaseName;

		public void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration _)
		{
			services.AddDbContext<IYuGetDbContext, MySQLDbContext>(x => {
				x.UseMySql(options.Database.ConnectionString, ServerVersion.AutoDetect(options.Database.ConnectionString));
			});
		}
	}
}
