using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YuGet.Base;
using YuGet.Base.Models;

namespace YuGet.Core
{
	internal sealed class MirrorV3Client : IMirrorNuGetClient
    {
        private readonly NuGetClient _client;

        public MirrorV3Client(NuGetClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Stream> DownloadPackageAsync(string id, NuGetVersion version, CancellationToken cancellationToken)
        {
            return await _client.DownloadPackageAsync(id, version, cancellationToken);
        }

        public async Task<IReadOnlyList<PackageMetadata>> GetPackageMetadataAsync(string id, CancellationToken cancellationToken)
        {
            return await _client.GetPackageMetadataAsync(id, cancellationToken);
        }

        public async Task<IReadOnlyList<NuGetVersion>> ListPackageVersionsAsync(string id, bool includeUnlisted, CancellationToken cancellationToken)
        {
            return await _client.ListPackageVersionsAsync(id, includeUnlisted, cancellationToken);
        }
    }
}
