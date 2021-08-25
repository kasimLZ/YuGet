using YuGet.Core.Builder;

namespace YuGet.Storage.AWS
{
	public static class DependencyInjectionExtensions
	{
		public static IYuGetOptionBuilder AddAWS(this IYuGetOptionBuilder builder) => builder.AddStorage<AzureStorageProvider>();
	}
}
