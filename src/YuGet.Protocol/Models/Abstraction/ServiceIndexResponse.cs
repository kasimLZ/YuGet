using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The entry point for a NuGet package source used by the client to discover NuGet APIs.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/overview
    /// </summary>
    public abstract class ServiceIndexResponse
    {
        /// <summary>
        /// The service index's version.
        /// </summary>
        [JsonPropertyName("version")]
        public virtual string Version { get; set; }

        /// <summary>
        /// The resources declared by this service index.
        /// </summary>
        [JsonPropertyName("resources")]
        public virtual IReadOnlyList<ServiceIndexItem> Resources { get; set; }
    }
}
