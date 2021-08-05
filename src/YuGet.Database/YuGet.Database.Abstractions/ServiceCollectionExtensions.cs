using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using YuGet.Database;
using YuGet.Database.Abstractions;
using YuGet.Database.Abstractions.Infrastructure;
using YuGet.Database.Abstractions.Options;

namespace YuGet
{
	public static class ServiceCollectionExtensions
	{
		private static readonly Dictionary<Type, IYuGetDbContextProvider> dbContextProviders = new Dictionary<Type, IYuGetDbContextProvider>();
		 
		public static IServiceCollection AddYuGetDbContextCore(this IServiceCollection services) 
		{
			using var provider =  services.BuildServiceProvider();

			var configuraion = provider.GetRequiredService<IConfiguration>();
			
			services.Configure<YuGetDatabaseOption>(configuraion.GetSection(YuGetDatabaseOption.SectionName));

			services.TryAddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));

			return services;
		}

		public static IServiceCollection AddYuGetDbContext(this IServiceCollection services)
		{
			services.AddYuGetDbContextCore();

			using var provider = services.BuildServiceProvider();

			var option = provider.GetRequiredService<IOptionsSnapshot<YuGetDatabaseOption>>();

			Console.WriteLine(option.Value.DatabaseType + "============" + option.Value.ConnectString);

			var dbProvider = dbContextProviders.Values.FirstOrDefault(a => a.DatabaseName.Equals(option.Value.DatabaseType, StringComparison.OrdinalIgnoreCase));

			if (dbProvider == null)
			{
				throw new NullReferenceException($"Can not found any DbContext named \"{option.Value.DatabaseType}\"");
			}

			dbProvider.SetupDbContext(services, option.Value);

			return services;
		}

		public static IServiceCollection AddYuGetDbContextProvider<TProvider>(this IServiceCollection services)
			where TProvider : class, IYuGetDbContextProvider, new()
		{
			var provider = new TProvider();

			dbContextProviders.TryAdd(provider.GetType(), provider);

			return services;
		}
	}
}
