using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// A single package type from a <see cref="SearchResult"/>.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/search-query-service-resource#search-result
    /// </summary>
    public abstract class SearchResultPackageType
    {
        /// <summary>
        /// The name of the package type.
        /// </summary>
        [JsonPropertyName("name")]
        public virtual string Name { get; set; }
    }
}
