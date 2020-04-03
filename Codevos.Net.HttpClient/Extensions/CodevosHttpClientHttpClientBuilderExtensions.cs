using System;
using System.Net.Http;
using System.Threading;
using Codevos.Net.HttpClient;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides <see cref="IHttpClientBuilder"/> extension methods.
    /// </summary>
    public static class CodevosHttpClientHttpClientBuilderExtensions
    {
        /// <summary>
        /// Registers an action to invoke before a <see cref="HttpRequestMessage"/> is sent by calling <see cref="HttpClientBuilderExtensions.AddHttpMessageHandler(IHttpClientBuilder, Func{IServiceProvider, DelegatingHandler})"/>.
        /// </summary>
        /// <param name="httpClientBuilder">The HTTP client builder </param>
        /// <param name="beforeSend">The action to invoke before sending the <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder"/> to use for further configuration.</returns>
        public static IHttpClientBuilder ForeachRequest(
            this IHttpClientBuilder httpClientBuilder,
            Action<IServiceProvider, HttpRequestMessage, CancellationToken> beforeSend
        )
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));
            if (beforeSend == null) throw new ArgumentNullException(nameof(beforeSend));

            httpClientBuilder
                .AddHttpMessageHandler(serviceProvider => new HttpClientDelegatingHandler(
                    beforeSend,
                    () => serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices?.GetService<IServiceProvider>() ?? serviceProvider
                ));

            return httpClientBuilder;
        }

        /// <summary>
        /// Registers an action to invoke before a <see cref="HttpRequestMessage"/> is sent by calling <see cref="HttpClientBuilderExtensions.AddHttpMessageHandler(IHttpClientBuilder, Func{IServiceProvider, DelegatingHandler})"/>.
        /// </summary>
        /// <param name="httpClientBuilder">The HTTP client builder </param>
        /// <param name="beforeSend">The action to invoke before sending the <see cref="HttpRequestMessage"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder"/> to use for further configuration.</returns>
        public static IHttpClientBuilder ForeachRequest(
            this IHttpClientBuilder httpClientBuilder,
            Action<HttpRequestMessage, CancellationToken> beforeSend
        )
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));
            if (beforeSend == null) throw new ArgumentNullException(nameof(beforeSend));

            httpClientBuilder
                .AddHttpMessageHandler(serviceProvider => new HttpClientDelegatingHandler(
                    (serviceProvider, request, cancellationToken) => beforeSend.Invoke(request, cancellationToken)
                ));

            return httpClientBuilder;
        }
    }
}
