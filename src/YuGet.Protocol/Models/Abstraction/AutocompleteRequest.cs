namespace YuGet.Core.Models.Abstraction
{
    /// <summary>
    /// The NuGet V3 search request.
    /// See: https://docs.microsoft.com/en-us/nuget/api/search-autocomplete-service-resource#request-parameters
    /// </summary>
    public abstract class AutocompleteRequest
    {
        /// <summary>
        /// The number of results to skip, for pagination.
        /// </summary>
        public virtual int Skip { get; set; }

        /// <summary>
        /// The number of results to return, for pagination.
        /// </summary>
        public virtual int Take { get; set; }

        /// <summary>
        /// Whether to include pre-release packages.
        /// </summary>
        public virtual bool IncludePrerelease { get; set; }

        /// <summary>
        /// Whether to include SemVer 2.0.0 compatible packages.
        /// </summary>
        public virtual bool IncludeSemVer2 { get; set; }

        /// <summary>
        /// Filter results to a package type. If null, no filter is applied.
        /// </summary>
        public virtual string PackageType { get; set; }

        /// <summary>
        /// The search query.
        /// </summary>
        public virtual string Query { get; set; }
    }
}
