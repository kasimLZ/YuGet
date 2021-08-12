using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// A resource in the <see cref="ServiceIndexResponse"/>.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/service-index#resources
    /// </summary>
    public abstract class ServiceIndexItem
    {
        /// <summary>
        /// The resource's base URL.
        /// </summary>
        [JsonPropertyName("@id")]
        public virtual string ResourceUrl { get; set; }

        /// <summary>
        /// The resource's type.
        /// </summary>
        [JsonPropertyName("@type")]
        public virtual string Type { get; set; }

        /// <summary>
        /// Human readable comments about the resource.
        /// </summary>
        [JsonPropertyName("comment")]
        public virtual string Comment { get; set; }
    }
}
