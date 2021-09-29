using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The alternate package that should be used instead of a deprecated package.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/registration-base-url-resource#package-deprecation
    /// </summary>
    public abstract class AlternatePackage
    {
        [JsonPropertyName("@id")]
        public virtual string Url { get; set; }

        [JsonPropertyName("@type")]
        public virtual string Type { get; set; }

        /// <summary>
        /// The ID of the alternate package.
        /// </summary>
        [JsonPropertyName("id")]
        public virtual string Id { get; set; }

        /// <summary>
        /// The allowed version range, or * if any version is allowed.
        /// </summary>
        [JsonPropertyName("range")]
        public virtual string Range { get; set; }
        
    }
}
