using YuGet.Core.Builder;

namespace YuGet.Storage.Aliyun
{
	public static class DependencyInjectionExtensions
	{
		public static IYuGetOptionBuilder AddAliyunOSS(this IYuGetOptionBuilder builder) => builder.AddStorage<AliyunOssStorageProvider>();
	}
}
