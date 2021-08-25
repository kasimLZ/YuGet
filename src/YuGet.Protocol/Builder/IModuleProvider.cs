using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;

namespace YuGet.Protocol.Builder
{
	public interface IModuleProvider
	{
		string Sign { get; }

		void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration);
	}
}
