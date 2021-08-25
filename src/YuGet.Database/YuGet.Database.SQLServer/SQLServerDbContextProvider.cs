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

		public void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration _)
		{
			services.AddDbContext<IYuGetDbContext, SQLServerDbContext>(x => {
				x.UseSqlServer(options.Database.ConnectionString);
			});
		}
	}
}
