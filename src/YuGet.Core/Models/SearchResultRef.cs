using System.Collections.Generic;
using System.Text.Json.Serialization;
using YuGet.Core.Internal;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Models
{
    /// <inheritdoc cref="SearchResult"/>
    internal class SearchResultRef : SearchResult
    {
        /// <summary>
        /// The authors of the matched package.
        /// </summary>
        [JsonPropertyName("authors")]
        [JsonConverter(typeof(StringOrStringArrayJsonConverter))]
        public override IReadOnlyList<string> Authors { get; set; }
    }
}
