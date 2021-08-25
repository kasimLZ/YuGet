using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;

namespace YuGet.Protocol.Builder
{
	public interface IModuleProvider
	{
		ModuleProviderType ModuleType { get; } 

		string Sign { get; }

		void SetupModule(IServiceCollection services, YuGetOptions options);
	}
}
