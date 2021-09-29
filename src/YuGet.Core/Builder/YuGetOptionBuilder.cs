using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using YuGet.Protocol.Builder;

namespace YuGet.Core.Builder
{
	internal class YuGetOptionBuilder : IYuGetOptionBuilder
	{
		private readonly Dictionary<ModuleProviderType, Dictionary<string, IModuleProvider>> RegistedModuleProvider = new();

		public IServiceCollection Service { get; set; }

		public YuGetOptions Options { get; set; }

		public IConfiguration Configuration { get; set; }

		public IYuGetOptionBuilder AddModuleProvider<TProvider>(ModuleProviderType module, TProvider provider) where TProvider : IModuleProvider
		{
			if (!Enum.IsDefined(module))
			{
				throw new ArgumentException($"Unacceptable module type :{module}");
			}

			if (provider == null)
			{
				throw new ArgumentNullException(nameof(provider));
			}

			this[module, provider.Sign] = provider;

			return this;
		}

		public IYuGetOptionBuilder AddModuleProvider<TProvider>(ModuleProviderType module)
			where TProvider : IModuleProvider, new()
		{
			return AddModuleProvider(module, new TProvider());
		}

		internal string this[ModuleProviderType module]
		{
			get => module switch
			{
				ModuleProviderType.Database => Options.Database.Type,
				ModuleProviderType.Stroage => Options.Storage.Type,
				ModuleProviderType.Authentication => "None",
				ModuleProviderType.Host => "API,UI",
				_ => null,
			};
		}

		internal IModuleProvider this[ModuleProviderType module, string moduleName]
		{
			get
			{
				if (RegistedModuleProvider.TryGetValue(module, out var list))
				{
					if (list.TryGetValue(moduleName, out var provider))
					{
						return provider;
					}
				}
				return null;
			}

			private set
			{
				if (!RegistedModuleProvider.TryGetValue(module, out var map))
				{
					RegistedModuleProvider.Add(module, map = new());
				}

				if (map.ContainsKey(moduleName))
				{
					throw new Exception($"The module \"{moduleName}\" for {module} is aready installed.");
				}

				map.Add(moduleName, value);
			}
		}
	}
}
