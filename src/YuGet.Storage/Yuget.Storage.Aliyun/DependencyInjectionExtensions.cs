using YuGet.Core.Builder;
using YuGet.Storage.Aliyun;

namespace YuGet.Storage
{
	public static class DependencyInjectionExtensions
	{
		public static IYuGetOptionBuilder AddAliyunOSS(this IYuGetOptionBuilder builder) => builder.AddStorage<AliyunOssStorageProvider>();
	}
}
