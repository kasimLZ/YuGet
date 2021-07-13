using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace YuGet.Storage
{
	internal static class StreamExtensions
	{
        public static bool Matches(this Stream content, Stream target)
        {
            using (var sha256 = SHA256.Create())
            {
                var contentHash = sha256.ComputeHash(content);
                var targetHash = sha256.ComputeHash(target);

                return contentHash.SequenceEqual(targetHash);
            }
        }
    }
}
