using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace System.Net.Http
{
    /// <summary>
    /// Provides <see cref="HttpClient"/> extensions methods.
    /// </summary>
    public static class CodevosHttpClientHttpClientExtensions
    {
        /// <summary>
        /// HTTP POSTs the given model as JSON in the HTTP request body.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make the call with.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="value">The model.</param>
        /// <param name="jsonSerializerOptions">Optional <see cref="JsonSerializerOptions"/> to use for JSON serialization.</param>
        /// <param name="cancellationToken">A cancellation token to cancel running tasks with.</param>
        /// <returns>The HTTP response to the HTTP request.</returns>
        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T value, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            return await SendAsJsonAsync(
                httpClient,
                HttpMethod.Post,
                requestUri,
                value,
                jsonSerializerOptions,
                cancellationToken
            );
        }

        /// <summary>
        /// HTTP PUTs the given model as JSON in the HTTP request body.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make the call with.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="value">The model.</param>
        /// <param name="jsonSerializerOptions">Optional <see cref="JsonSerializerOptions"/> to use for JSON serialization.</param>
        /// <param name="cancellationToken">A cancellation token to cancel running tasks with.</param>
        /// <returns>The HTTP response to the HTTP request.</returns>
        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T value, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            return await SendAsJsonAsync(
                httpClient,
                HttpMethod.Put,
                requestUri,
                value,
                jsonSerializerOptions,
                cancellationToken
            );
        }

        /// <summary>
        /// HTTP PATCHes the given model as JSON in the HTTP request body.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make the call with.</param>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="value">The model.</param>
        /// <param name="jsonSerializerOptions">Optional <see cref="JsonSerializerOptions"/> to use for JSON serialization.</param>
        /// <param name="cancellationToken">A cancellation token to cancel running tasks with.</param>
        /// <returns>The HTTP response to the HTTP request.</returns>
        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T value, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            return await SendAsJsonAsync(
                httpClient,
                HttpMethod.Patch,
                requestUri,
                value,
                jsonSerializerOptions,
                cancellationToken
            );
        }

        private static async Task<HttpResponseMessage> SendAsJsonAsync<T>(HttpClient httpClient, HttpMethod httpMethod, string requestUri, T value, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
        {
            using (var jsonContent = await CreateJsonContent(value, jsonSerializerOptions, cancellationToken))
            using (var httpRequestMessage = CreateHttpRequestMessage(httpMethod, requestUri, jsonContent))
            {
                return await httpClient.SendAsync(httpRequestMessage, cancellationToken);
            }
        }

        private static HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string requestUri, HttpContent httpContent)
        {
            return new HttpRequestMessage(httpMethod, requestUri)
            {
                Content = httpContent
            };
        }

        private static async Task<StringContent> CreateJsonContent<T>(T value, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
        {
            string json;

            using (var memoryStream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(memoryStream, value, jsonSerializerOptions, cancellationToken);

                using (var streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    memoryStream.Position = 0;
                    json = await streamReader.ReadToEndAsync();
                }
            }

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
