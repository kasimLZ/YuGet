using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.Abstractions;
using YuGet.Database.Abstractions.Options;

namespace YuGet.Database.PostgreSQL
{
	internal sealed class PostgreSQLDbContextProvider : IYuGetDbContextProvider
	{
		public string DatabaseName => "PostgreSQL";

		public void SetupDbContext(IServiceCollection services, YuGetDatabaseOption option)
		{
			services.AddDbContext<IYuGetDbContext, PostgreSQLDbContext>(x => {
				x.UseNpgsql(option.ConnectString);
			});
		}
	}
}
