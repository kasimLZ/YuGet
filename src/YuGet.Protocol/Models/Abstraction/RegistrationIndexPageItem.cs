using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// An item in the <see cref="CatalogIndex"/> that references a <see cref="CatalogLeaf"/>.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/registration-base-url-resource#registration-leaf-object-in-a-page
    /// </summary>
    public abstract class RegistrationIndexPageItem
    {
        /// <summary>
        /// The URL to the registration leaf.
        /// </summary>
        [JsonPropertyName("@id")]
        public virtual string RegistrationLeafUrl { get; set; }

        /// <summary>
        /// The catalog entry containing the package metadata.
        /// </summary>
        [JsonPropertyName("catalogEntry")]
        public virtual PackageMetadata PackageMetadata { get; set; }

        /// <summary>
        /// The URL to the package content (.nupkg)
        /// </summary>
        [JsonPropertyName("packageContent")]
        public virtual string PackageContentUrl { get; set; }
    }
}
