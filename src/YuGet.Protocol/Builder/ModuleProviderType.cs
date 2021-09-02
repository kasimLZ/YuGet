using System.ComponentModel.DataAnnotations;

namespace YuGet.Protocol.Builder
{
	public enum ModuleProviderType
	{
		[Required]
		Database = 1,

		[Required]
		Stroage = 2,

		[Required]
		Authentication = 3,

		Host = 4,
		
		UI = 5,
	}
}
