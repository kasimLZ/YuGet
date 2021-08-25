using YuGet.Protocol.Builder;

namespace YuGet.Core.Builder
{
	public interface IYuGetOptionBuilder
	{
		IYuGetOptionBuilder AddModuleProvider<TProvider, TInstaller>() where TProvider : IModuleProvider where TInstaller: TProvider, new();

		IYuGetOptionBuilder AddModuleProvider<TProvider>(TProvider provider) where TProvider : IModuleProvider;
	}
}
