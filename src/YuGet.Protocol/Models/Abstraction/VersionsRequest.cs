namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The NuGet V3 enumerate package versions request.
    /// See: https://docs.microsoft.com/en-us/nuget/api/search-autocomplete-service-resource#request-parameters-1
    /// </summary>
    public abstract class VersionsRequest
    {
        /// <summary>
        /// Whether to include pre-release packages.
        /// </summary>
        public virtual bool IncludePrerelease { get; set; }

        /// <summary>
        /// Whether to include SemVer 2.0.0 compatible packages.
        /// </summary>
        public virtual bool IncludeSemVer2 { get; set; }

        /// <summary>
        /// The package ID whose versions should be fetched.
        /// </summary>
        public virtual string PackageId { get; set; }
    }
}
