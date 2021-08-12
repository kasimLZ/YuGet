using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The response to a search query.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/search-query-service-resource#response
    /// </summary>
    public abstract class SearchResponse
    {
        [JsonPropertyName("@context")]
        public virtual SearchContext Context { get; set; }

        /// <summary>
        /// The total number of matches, disregarding skip and take.
        /// </summary>
        [JsonPropertyName("totalHits")]
        public virtual long TotalHits { get; set; }

        /// <summary>
        /// The packages that matched the search query.
        /// </summary>
        [JsonPropertyName("data")]
        public virtual IReadOnlyList<SearchResult> Data { get; set; }
    }
}
