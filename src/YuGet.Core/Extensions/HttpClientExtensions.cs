using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace YuGet.Core
{
    internal static class HttpClientExtensions
    {
		public static IServiceCollection AddHttpClientService(this IServiceCollection services)
		{
			services.TryAddSingleton(provider => {

				var options = provider.GetRequiredService<IOptions<MirrorOptions>>().Value;

				var client = new HttpClient(new HttpClientHandler
				{
					AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				});

				client.DefaultRequestHeaders.Add("User-Agent", "Dotnet 5.0/yuget-1.0.0");
				client.Timeout = TimeSpan.FromSeconds(options.PackageDownloadTimeoutSeconds);

				return client;
			});

			return services;
		}


		/// <summary>
		/// Deserialize JSON content.
		/// </summary>
		/// <typeparam name="TResult">The JSON type to deserialize.</typeparam>
		/// <param name="httpClient">The HTTP client that will perform the request.</param>
		/// <param name="requestUri">The request URI.</param>
		/// <param name="cancellationToken">A token to cancel the task.</param>
		/// <returns>The deserialized JSON content</returns>
		public static async Task<TResult> GetFromJsonAsync<TResult>(
            this HttpClient httpClient,
            string requestUri,
            CancellationToken cancellationToken = default)
        {
			using var response = await httpClient.GetAsync(
				requestUri,
				HttpCompletionOption.ResponseHeadersRead,
				cancellationToken);
			// This is similar to System.Net.Http.Json's implementation, however,
			// this does not validate that the response's content type indicates JSON content.
			response.EnsureSuccessStatusCode();

			using var stream = await response.Content.ReadAsStreamAsync();
			return await JsonSerializer.DeserializeAsync<TResult>(stream, cancellationToken: cancellationToken);
		}

        /// <summary>
        /// Deserialize JSON content. If the HTTP response status code is 404,
        /// returns the default value.
        /// </summary>
        /// <typeparam name="TResult">The JSON type to deserialize.</typeparam>
        /// <param name="httpClient">The HTTP client that will perform the request.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="cancellationToken">A token to cancel the task.</param>
        /// <returns>The JSON content, or, the default value if the HTTP response status code is 404.</returns>
        public static async Task<TResult> GetFromJsonOrDefaultAsync<TResult>(
            this HttpClient httpClient,
            string requestUri,
            CancellationToken cancellationToken = default)
        {
			using var response = await httpClient.GetAsync(
				requestUri,
				HttpCompletionOption.ResponseHeadersRead,
				cancellationToken);
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return default;
			}

			response.EnsureSuccessStatusCode();

			using (var stream = await response.Content.ReadAsStreamAsync())
			{
				return await JsonSerializer.DeserializeAsync<TResult>(stream);
			}
		}
    }
}
