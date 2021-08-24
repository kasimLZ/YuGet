using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Models
{
	internal class SearchContextRef : SearchContext
    {
        public static SearchContextRef Default(string registrationBaseUrl)
        {
            return new SearchContextRef
            {
                Vocab = "http://schema.nuget.org/schema#",
                Base = registrationBaseUrl
            };
        }
    }
}
