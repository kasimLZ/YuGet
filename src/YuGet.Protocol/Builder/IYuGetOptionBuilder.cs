using YuGet.Protocol.Builder;

namespace YuGet.Core.Builder
{
	public interface IYuGetOptionBuilder
	{
		IYuGetOptionBuilder AddModuleProvider<TProvider>(ModuleProviderType module) where TProvider : IModuleProvider, new();

		IYuGetOptionBuilder AddModuleProvider<TProvider>(ModuleProviderType module, TProvider provider) where TProvider : IModuleProvider;
	}
}
