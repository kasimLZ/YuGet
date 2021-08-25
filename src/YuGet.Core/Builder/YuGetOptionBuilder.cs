using Microsoft.Extensions.Configuration;
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

		public IConfiguration Configuration { get; set; }

		public IYuGetOptionBuilder AddModuleProvider<TProvider>(ModuleProviderType module, TProvider provider) where TProvider : IModuleProvider
		{
			if (!Enum.IsDefined(module))
			{
				throw new ArgumentException($"Unacceptable module type :{module}");
			}

			if ((module & Installed) > 0)
			{
				throw new ArgumentException($"The {module} is Installed.");
			}

			try
			{
				Installed |= module;
				provider.SetupModule(Service, Options, Configuration);
			}
			catch (Exception e)
			{
				Installed &= ~module;
			}
			
			return this;
		}

		public IYuGetOptionBuilder AddModuleProvider<TProvider>(ModuleProviderType module)
			where TProvider : IModuleProvider, new()
		{
			AddModuleProvider(module, new TProvider());
			return this;
		}
	}
}
