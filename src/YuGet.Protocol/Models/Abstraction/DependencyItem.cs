using System.Text.Json.Serialization;

namespace YuGet.Core.Models.Abstraction
{
	/// <summary>
	/// Represents a package dependency.
	///
	/// See https://docs.microsoft.com/en-us/nuget/api/registration-base-url-resource#package-dependency
	/// </summary>
	public abstract class DependencyItem
    {
        /// <summary>
        /// The ID of the package dependency.
        /// </summary>
        [JsonPropertyName("id")]
        public virtual string Id { get; set; }

        /// <summary>
        /// The allowed version range of the dependency.
        /// </summary>
        [JsonPropertyName("range")]
        public virtual string Range { get; set; }
    }
}
