using YuGet.Core.Models.Abstraction;

namespace YuGet.Storage.Azure.Models
{
	internal class AzureSearchContext : SearchContext
    {
        public static AzureSearchContext Default(string registrationBaseUrl)
        {
            return new AzureSearchContext
            {
                Vocab = "http://schema.nuget.org/schema#",
                Base = registrationBaseUrl
            };
        }
    }
}
