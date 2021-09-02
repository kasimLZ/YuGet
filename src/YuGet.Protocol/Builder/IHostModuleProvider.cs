using Microsoft.AspNetCore.Builder;

namespace YuGet.Protocol.Builder
{
	public interface IHostModuleProvider : IModuleProvider
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		void ConfigModule(IApplicationBuilder app);
	}
}
