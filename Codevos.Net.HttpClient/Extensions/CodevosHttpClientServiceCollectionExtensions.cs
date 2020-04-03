using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides <see cref="IServiceCollection"/> extension methods for registering HTTP clients.
    /// </summary>
    public static class CodevosHttpClientServiceCollectionExtensions
    {
        /// <summary>
        /// Adds HTTP clients to the <see cref="IServiceCollection"/> by calling <see cref="HttpClientFactoryServiceCollectionExtensions.AddHttpClient(IServiceCollection, string, Action{System.Net.Http.HttpClient})"/>.
        /// </summary>
        /// <param name="services">The service collection to add the HTTP clients to.</param>
        /// <param name="httpClientDictionary">The dictionary of HTTP client name / base address pairs.</param>
        /// <param name="httpClientAdded">An optional action to invoke when the HTTP client is added to the service collection.</param>
        /// <returns>The <see cref="IServiceCollection"/> to use for further configuration.</returns>
        public static IServiceCollection AddHttpClients(
            this IServiceCollection services,
            IDictionary<string, string> httpClientDictionary,
            Action<string, string, IHttpClientBuilder> httpClientAdded = null
        )
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (httpClientDictionary == null) throw new ArgumentNullException(nameof(httpClientDictionary));
            if (httpClientDictionary.Count == 0) return services;

            foreach (var pair in httpClientDictionary)
            {
                var restClientSettings = pair.Value;

                var httpClientBuilder = services
                    .AddHttpClient(
                        pair.Key,
                        client => client.BaseAddress = new Uri(pair.Value)
                    );                   

                httpClientAdded?.Invoke(pair.Key, pair.Value, httpClientBuilder);
            }

            return services;
        }
    }
}