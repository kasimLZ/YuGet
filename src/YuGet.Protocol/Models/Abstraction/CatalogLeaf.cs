using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
    // This classed is based off: https://github.com/NuGet/NuGet.Services.Metadata/blob/64af0b59c5a79e0143f0808b39946df9f16cb2e7/src/NuGet.Protocol.Catalog/Models/CatalogLeaf.cs

    /// <summary>
    /// A catalog leaf. Represents a single package event.
    /// Leafs can be discovered from a <see cref="CatalogPage"/>.
    /// 
    /// See https://docs.microsoft.com/en-us/nuget/api/catalog-resource#catalog-leaf
    /// </summary>
    public abstract class CatalogLeaf : ICatalogLeafItem
    {
        /// <summary>
        /// The URL to the current catalog leaf.
        /// </summary>
        [JsonPropertyName("@id")]
        public virtual string CatalogLeafUrl { get; set; }

        /// <summary>
        /// The type of the current catalog leaf.
        /// </summary>
        [JsonPropertyName("@type")]
        public virtual IReadOnlyList<string> Type { get; set; }

        /// <summary>
        /// The catalog commit ID associated with this catalog item.
        /// </summary>
        [JsonPropertyName("catalog:commitId")]
        public virtual string CommitId { get; set; }

        /// <summary>
        /// The commit timestamp of this catalog item.
        /// </summary>
        [JsonPropertyName("catalog:commitTimeStamp")]
        public virtual DateTimeOffset CommitTimestamp { get; set; }

        /// <summary>
        /// The package ID of the catalog item.
        /// </summary>
        [JsonPropertyName("id")]
        public virtual string PackageId { get; set; }

        /// <summary>
        /// The published date of the package catalog item.
        /// </summary>
        [JsonPropertyName("published")]
        public virtual DateTimeOffset Published { get; set; }

        /// <summary>
        /// The package version of the catalog item.
        /// </summary>
        [JsonPropertyName("version")]
        public virtual string PackageVersion { get; set; }
    }
}
