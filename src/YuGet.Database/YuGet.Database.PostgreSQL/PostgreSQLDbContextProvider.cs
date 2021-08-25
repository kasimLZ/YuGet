using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Database.Abstractions;
using YuGet.Protocol.Builder;

namespace YuGet.Database.PostgreSQL
{
	internal sealed class PostgreSQLDbContextProvider : IYuGetDbContextProvider
	{
		private const string DatabaseName = "PostgreSQL";

		public ModuleProviderType ModuleType => ModuleProviderType.Database;

		public string Sign => DatabaseName;

		public void SetupModule(IServiceCollection services, YuGetOptions options)
		{
			services.AddDbContext<IYuGetDbContext, PostgreSQLDbContext>(x => {
				x.UseNpgsql(options.Database.ConnectionString);
			});
		}
	}
}
