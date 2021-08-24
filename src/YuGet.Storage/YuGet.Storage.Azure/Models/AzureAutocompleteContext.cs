using YuGet.Core.Models.Abstraction;

namespace YuGet.Storage.Azure.Models
{
	internal class AzureAutocompleteContext : AutocompleteContext
	{
		public static readonly AutocompleteContext Default = new AzureAutocompleteContext
		{
			Vocab = "http://schema.nuget.org/schema#"
		};
	}
}
