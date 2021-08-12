using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    public abstract class SearchContext
    {
        [JsonPropertyName("@vocab")]
        public virtual string Vocab { get; set; }

        [JsonPropertyName("@base")]
        public virtual string Base { get; set; }
    }
}
