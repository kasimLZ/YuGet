using System;
using Aliyun.OSS;
using YuGet.Storage.Aliyun;
using YuGet.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace YuGet
{
    public static class AliyunApplicationExtensions
    {
		//public static BaGetApplication AddAliyunOssStorage(this IServiceCollection services)
		//{
		//	services.AddBaGetOptions<AliyunStorageOptions>(nameof(BaGetOptions.Storage));

		//	services.AddTransient<AliyunStorageService>();
		//	services.TryAddTransient<IStorageService>(provider => provider.GetRequiredService<AliyunStorageService>());

		//	app.Services.AddSingleton(provider =>
		//	{
		//		var options = provider.GetRequiredService<IOptions<AliyunStorageOptions>>().Value;

		//		return new OssClient(options.Endpoint, options.AccessKey, options.AccessKeySecret);
		//	});

		//	app.Services.AddProvider<IStorageService>((provider, config) =>
		//	{
		//		if (!config.HasStorageType("AliyunOss")) return null;

		//		return provider.GetRequiredService<AliyunStorageService>();
		//	});

		//	return app;
		//}

		//public static BaGetApplication AddAliyunOssStorage(
		//	this BaGetApplication app,
		//	Action<AliyunStorageOptions> configure)
		//{
		//	app.AddAliyunOssStorage();
		//	app.Services.Configure(configure);
		//	return app;
		//}
	}
}
