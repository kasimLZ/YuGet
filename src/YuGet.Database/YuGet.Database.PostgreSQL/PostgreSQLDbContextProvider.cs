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

		public void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration _)
		{
			services.AddDbContext<IYuGetDbContext, PostgreSQLDbContext>(x => {
				x.UseNpgsql(options.Database.ConnectionString);
			});
		}
	}
}
