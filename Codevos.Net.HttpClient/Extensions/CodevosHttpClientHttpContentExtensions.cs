using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// Provides <see cref="HttpContent"/> extension methods.
    /// </summary>
    public static class CodevosHttpClientHttpContentExtensions
    {
        /// <summary>
        /// Deserializes the <see cref="HttpContent"/> to the given type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="httpContent">The <see cref="HttpContent"/> containing the JSON.</param>
        /// <param name="jsonSerializerOptions">Optional <see cref="JsonSerializerOptions"/> to use for JSON serialization.</param>
        /// <param name="cancellationToken">A cancellation token to cancel running tasks with.</param>
        /// <returns>The deserialized JSON content.</returns>
        public static async Task<T> ReadAsAsync<T>(this HttpContent httpContent, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default)
        {
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken);
            }
        }
    }
}
