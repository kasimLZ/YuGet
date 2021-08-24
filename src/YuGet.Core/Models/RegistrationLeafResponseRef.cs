using System.Collections.Generic;
using YuGet.Core.Models.Abstraction;

namespace YuGet.Core.Models
{
	/// <inheritdoc cref="RegistrationLeafResponse"/>
	internal class RegistrationLeafResponseRef : RegistrationLeafResponse
    {
        public static readonly IReadOnlyList<string> DefaultType = new List<string>
        {
            "Package",
            "http://schema.nuget.org/catalog#Permalink"
        };
    }
}
