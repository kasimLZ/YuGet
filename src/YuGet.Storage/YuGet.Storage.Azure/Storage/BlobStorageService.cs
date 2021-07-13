using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace YuGet.Storage.Azure
{
    // See: https://github.com/NuGet/NuGetGallery/blob/master/src/NuGetGallery.Core/Services/CloudBlobCoreFileStorageService.cs
    public class BlobStorageService : IStorageService
    {
        private readonly CloudBlobContainer _container;

        public BlobStorageService(CloudBlobContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public async Task<Stream> GetAsync(string path, CancellationToken cancellationToken)
        {
            return await _container
                .GetBlockBlobReference(path)
                .OpenReadAsync(cancellationToken);
        }

        public Task<Uri> GetDownloadUriAsync(string path, CancellationToken cancellationToken)
        {
            // TODO: Make expiry time configurable.
            var blob = _container.GetBlockBlobReference(path);
            var accessPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(10)),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var signature = blob.GetSharedAccessSignature(accessPolicy);
            var result = new Uri(blob.Uri, signature);

            return Task.FromResult(result);
        }

        public async Task<StoragePutResult> PutAsync(
            string path,
            Stream content,
            string contentType,
            CancellationToken cancellationToken)
        {
            var blob = _container.GetBlockBlobReference(path);
            var condition = AccessCondition.GenerateIfNotExistsCondition();

            blob.Properties.ContentType = contentType;

            try
            {
                await blob.UploadFromStreamAsync(
                    content,
                    condition,
                    options: null,
                    operationContext: null,
                    cancellationToken: cancellationToken);

                return StoragePutResult.Success;
            }
            catch (StorageException e) when (e.IsAlreadyExistsException())
            {
                using (var targetStream = await blob.OpenReadAsync(cancellationToken))
                {
                    content.Position = 0;
                    return Matches(content,targetStream)
                        ? StoragePutResult.AlreadyExists
                        : StoragePutResult.Conflict;
                }
            }
        }

        public async Task DeleteAsync(string path, CancellationToken cancellationToken)
        {
            await _container
                .GetBlockBlobReference(path)
                .DeleteIfExistsAsync(cancellationToken);
        }

        public static bool Matches(Stream content, Stream target)
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
