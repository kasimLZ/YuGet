using System.Collections.Generic;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Models
{
    /// <inheritdoc cref="RegistrationIndexResponse"/>
    internal class RegistrationIndexResponseRef : RegistrationIndexResponse
    {
        public static readonly IReadOnlyList<string> DefaultType = new List<string>
        {
            "catalog:CatalogRoot",
            "PackageRegistration",
            "catalog:Permalink"
        };
    }
}
