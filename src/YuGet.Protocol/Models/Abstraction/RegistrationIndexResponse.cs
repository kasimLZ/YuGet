using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The metadata for a package and all of its versions.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/registration-base-url-resource#registration-index
    /// </summary>
    public abstract class RegistrationIndexResponse
    {
        /// <summary>
        /// The URL to the registration index.
        /// </summary>
        [JsonPropertyName("@id")]
        public virtual string RegistrationIndexUrl { get; set; }

        /// <summary>
        /// The registration index's type.
        /// </summary>
        [JsonPropertyName("@type")]
        public virtual IReadOnlyList<string> Type { get; set; }

        /// <summary>
        /// The number of registration pages. See <see cref="Pages"/>.
        /// </summary>
        [JsonPropertyName("count")]
        public virtual int Count { get; set; }

        /// <summary>
        /// The pages that contain all of the versions of the package, ordered
        /// by the package's version.
        /// </summary>
        [JsonPropertyName("items")]
        public virtual IReadOnlyList<RegistrationIndexPage> Pages { get; set; }
    }
}
