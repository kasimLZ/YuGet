using System.Collections.Generic;
using Microsoft.Azure.Search.Models;

namespace YuGet.Storage.Azure
{
    /// <summary>
    /// A custom analyzer for case insensitive exact match.
    /// </summary>
    public static class ExactMatchCustomAnalyzer
    {
        public const string Name = "yuget-exact-match-analyzer";

        public static CustomAnalyzer Instance = new CustomAnalyzer(
            Name,
            TokenizerName.Keyword,
            new List<TokenFilterName>
            {
                TokenFilterName.Lowercase
            });
    }
}
