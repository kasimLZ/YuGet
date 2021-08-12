using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    public abstract class AutocompleteContext
    {
        [JsonPropertyName("@vocab")]
        public virtual string Vocab { get; set; }
    }
}
