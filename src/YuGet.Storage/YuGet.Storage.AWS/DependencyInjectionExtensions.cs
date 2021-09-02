using YuGet.Core.Builder;
using YuGet.Storage.AWS;

namespace YuGet.Storage
{
	public static class DependencyInjectionExtensions
	{
		public static IYuGetOptionBuilder AddAWS(this IYuGetOptionBuilder builder) => builder.AddStorage<AWSStorageProvider>();
	}
}
