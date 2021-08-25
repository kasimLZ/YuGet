using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;
using YuGet.Storage.Abstractions;

namespace YuGet.Storage.Azure
{
	internal class AzureStorageProvider : IYuGetStorageProvider
	{
		public string Sign => "Azure";

		public void SetupModule(IServiceCollection services, YuGetOptions options)
		{
			
		}
	}
}
