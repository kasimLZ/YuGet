using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.Abstractions;
using YuGet.Database.Abstractions.Options;

namespace YuGet.Database.SQLServer
{
	internal sealed class SQLServerDbContextProvider : IYuGetDbContextProvider
	{
		public string DatabaseName => "Mssql";

		public void SetupDbContext(IServiceCollection services, YuGetDatabaseOption option)
		{
			services.AddDbContext<IYuGetDbContext, SQLServerDbContext>(x => {
				x.UseSqlServer(option.ConnectString);
			});

			//services.AddDbContext

		}

		delegate T2 sssss<in T1, out T2>(T1 t1);
	}
}
