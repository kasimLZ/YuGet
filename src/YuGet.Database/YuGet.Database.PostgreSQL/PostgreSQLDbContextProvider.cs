using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;

namespace YuGet.Database.PostgreSQL
{
	internal sealed class PostgreSQLDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "PostgreSQL";

		public string Sign => DatabaseName;

		public void RegistModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration)
		{
			services.AddDbContext<IYuGetDbContext, PostgreSQLDbContext>(x => {
				x.UseNpgsql(options.Database.ConnectionString);
			});
		}
	}
}
