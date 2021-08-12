using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The full list of versions for a single package.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/package-base-address-resource#enumerate-package-versions
    /// </summary>
    public abstract class PackageVersionsResponse
    {
        /// <summary>
        /// The versions, lowercased and normalized.
        /// </summary>
        [JsonPropertyName("versions")]
        public virtual IReadOnlyList<string> Versions { get; set; }
    }
}
