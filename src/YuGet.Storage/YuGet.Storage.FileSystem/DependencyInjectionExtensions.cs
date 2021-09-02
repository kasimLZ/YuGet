using YuGet.Core.Builder;
using YuGet.Storage.FileSystem;

namespace YuGet.Storage
{
	public static class DependencyInjectionExtensions
	{
		public static IYuGetOptionBuilder AddFileSystem(this IYuGetOptionBuilder builder) => builder.AddStorage<FileSystemStorageProvider>();
	}
}
