using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YuGet.Storage.Abstractions
{
	/// <summary>
	/// 
	/// </summary>
	public interface IYuGetStorageProvider
	{
		/// <summary>
		/// 
		/// </summary>
		string StroageName { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		void SetupStorage(IServiceCollection services, IConfiguration configuration);
	}
}
