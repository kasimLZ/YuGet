using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YuGet.Core;

namespace YuGet.Protocol.Builder
{
	/// <summary>
	/// 
	/// </summary>
	public interface IModuleProvider
	{
		/// <summary>
		/// 
		/// </summary>
		string Sign { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <param name="options"></param>
		/// <param name="configuration"></param>
		void RegistModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration);
	}
}
