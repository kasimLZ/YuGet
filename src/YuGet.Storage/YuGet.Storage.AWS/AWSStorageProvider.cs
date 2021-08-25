using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using YuGet.Core;
using YuGet.Storage.Abstractions;
using YuGet.Storage.Aws;

namespace YuGet.Storage.AWS
{
	internal class AWSStorageProvider : IYuGetStorageProvider
	{
		public string Sign => "AWS";

		public void SetupModule(IServiceCollection services, YuGetOptions options, IConfiguration configuration)
		{
			services.Configure<S3StorageOptions>(configuration.GetSection(nameof(options.Storage)));

			services.TryAddTransient<S3StorageService>();
			services.TryAddTransient<IStorageService>(provider => provider.GetRequiredService<S3StorageService>());

			services.AddSingleton(provider =>
			{
				var options = provider.GetRequiredService<IOptions<S3StorageOptions>>().Value;

				var config = new AmazonS3Config
				{
					RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region)
				};

				if (options.UseInstanceProfile)
				{
					var credentials = FallbackCredentialsFactory.GetCredentials();
					return new AmazonS3Client(credentials, config);
				}

				if (!string.IsNullOrEmpty(options.AssumeRoleArn))
				{
					var credentials = FallbackCredentialsFactory.GetCredentials();
					var assumedCredentials = AssumeRoleAsync(
							credentials,
							options.AssumeRoleArn,
							$"YuGet-Session-{Guid.NewGuid()}")
						.GetAwaiter()
						.GetResult();

					return new AmazonS3Client(assumedCredentials, config);
				}

				if (!string.IsNullOrEmpty(options.AccessKey))
				{
					return new AmazonS3Client(
						new BasicAWSCredentials(
							options.AccessKey,
							options.SecretKey),
						config);
				}

				return new AmazonS3Client(config);
			});
		}

		private static async Task<AWSCredentials> AssumeRoleAsync(
			AWSCredentials credentials,
			string roleArn,
			string roleSessionName)
		{
			var assumedCredentials = new AssumeRoleAWSCredentials(credentials, roleArn, roleSessionName);
			var immutableCredentials = await credentials.GetCredentialsAsync();

			if (string.IsNullOrWhiteSpace(immutableCredentials.Token))
			{
				throw new InvalidOperationException($"Unable to assume role {roleArn}");
			}

			return assumedCredentials;
		}
	}
}
