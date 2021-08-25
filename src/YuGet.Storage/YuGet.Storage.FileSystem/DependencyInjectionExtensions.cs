using YuGet.Core.Builder;

namespace YuGet.Storage.FileSystem
{
	public static class DependencyInjectionExtensions
	{
		public static IYuGetOptionBuilder AddFileSystem(this IYuGetOptionBuilder builder) => builder.AddStorage<FileSystemStorageProvider>();
	}
}
