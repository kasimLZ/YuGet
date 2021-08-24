using System.Text.Json.Serialization;
using YuGet.Core.Internal;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Models
{
    /// <inheritdoc cref=""/>
    internal class DependencyItemRef : DependencyItem
    {
        /// <inheritdoc/>
        [JsonPropertyName("range")]
        [JsonConverter(typeof(PackageDependencyRangeJsonConverter))]
        public override string Range { get; set; }
    }
}
