using Aliyun.OSS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using YuGet.Core;
using YuGet.Storage.Abstractions;

namespace YuGet.Storage.Aliyun
{
	internal class AliyunOssStorageProvider : IYuGetStorageProvider
	{
		public string Sign => "AliyunOss";

		public void RegistModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration)
		{
			services.Configure<AliyunStorageOptions>(configuration.GetSection(nameof(options.Storage)));

			services.TryAddTransient<AliyunStorageService>();
			services.TryAddTransient<IStorageService>(provider => provider.GetRequiredService<AliyunStorageService>());

			services.TryAddSingleton(provider =>
			{
				var options = provider.GetRequiredService<IOptions<AliyunStorageOptions>>().Value;

				return new OssClient(options.Endpoint, options.AccessKey, options.AccessKeySecret);
			});
		}
	}
}
