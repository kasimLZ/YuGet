using Microsoft.Extensions.DependencyInjection;
using YuGet.Database.Abstractions.Options;

namespace YuGet.Database.Abstractions
{
	public interface IYuGetDbContextProvider
	{
		string DatabaseName { get; }

		void SetupDbContext(IServiceCollection services, YuGetDatabaseOption option);
	}
}
