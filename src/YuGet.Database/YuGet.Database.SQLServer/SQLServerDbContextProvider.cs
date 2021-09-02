using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;

namespace YuGet.Database.SQLServer
{
	internal sealed class SQLServerDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "Mssql";

		public string Sign => DatabaseName;

		public void RegistModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration)
		{
			services.AddDbContext<IYuGetDbContext, SQLServerDbContext>(x => {
				x.UseSqlServer(options.Database.ConnectionString);
			});
		}
	}
}
