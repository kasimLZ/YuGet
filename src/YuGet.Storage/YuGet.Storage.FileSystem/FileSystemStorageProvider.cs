using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using YuGet.Core;
using YuGet.Storage.Abstractions;

namespace YuGet.Storage.FileSystem
{
	internal class FileSystemStorageProvider : IYuGetStorageProvider
	{
		public string Sign => "FileSystem";

		public void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration)
		{
			services.Configure<FileSystemStorageOptions>(configuration.GetSection(nameof(options.Storage)));
			services.TryAddTransient<IStorageService, FileStorageService>();
		}
	}
}
