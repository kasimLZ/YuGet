using Microsoft.Extensions.DependencyInjection;
using System;
using YuGet.Protocol.Builder;

namespace YuGet.Core.Builder
{
	internal class YuGetOptionBuilder : IYuGetOptionBuilder
	{
		private ModuleProviderType Installed = 0;

		public IServiceCollection Service { get; set; }

		public YuGetOptions Options { get; set; }

		public IYuGetOptionBuilder AddModuleProvider<TProvider>(TProvider provider) where TProvider : IModuleProvider
		{
			if (!Enum.IsDefined(provider.ModuleType))
			{
				throw new ArgumentException($"Unacceptable module type :{provider.ModuleType}");
			}

			if ((provider.ModuleType & Installed) > 0)
			{
				throw new ArgumentException($"The {provider.ModuleType} is Installed.");
			}
			Installed |= provider.ModuleType;

			provider.SetupModule(Service, Options);
			return this;
		}

		public IYuGetOptionBuilder AddModuleProvider<TProvider, TInstaller>()
			where TProvider : IModuleProvider
			where TInstaller : TProvider, new()
		{
			AddModuleProvider<TProvider>(new TInstaller());
			return this;
		}
	}
}
