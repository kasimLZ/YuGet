using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Models
{
	internal class AutocompleteContextRef : AutocompleteContext
    {
        public static readonly AutocompleteContextRef Default = new()
        {
            Vocab = "http://schema.nuget.org/schema#"
        };
    }
}
