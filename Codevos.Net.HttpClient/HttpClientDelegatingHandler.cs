using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Codevos.Net.HttpClient
{
    /// <summary>
    /// A HTTP client delegating handler that invokes an action just before a <see cref="HttpRequestMessage"/> is sent.
    /// The invoked action also provides optional access to the <see cref="IServiceProvider"/>.
    /// </summary>
    public class HttpClientDelegatingHandler : DelegatingHandler
    {
        private readonly Func<IServiceProvider> ServiceProviderFactory;
        private readonly Action<IServiceProvider, HttpRequestMessage, CancellationToken> BeforeSend;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientDelegatingHandler"/> class.
        /// </summary>
        /// <param name="beforeSend">The function to invoke just before a <see cref="HttpRequestMessage"/> is sent.</param>
        /// <param name="serviceProviderFactory">The optional function to invoke to get the <see cref="IServiceProvider"/>.</param>
        public HttpClientDelegatingHandler(Action<IServiceProvider, HttpRequestMessage, CancellationToken> beforeSend, Func<IServiceProvider> serviceProviderFactory = null)
        {
            BeforeSend = beforeSend ?? throw new ArgumentNullException(nameof(beforeSend));
            ServiceProviderFactory = serviceProviderFactory;
        }

        /// <summary>
        /// Invokes the 'before send' action and sends the <paramref name="request"/> to the server.
        /// </summary>
        /// <param name="request">The HTTP request to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel running tasks with.</param>
        /// <returns>The <see cref="HttpResponseMessage"/> to the HTTP request message.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            BeforeSend.Invoke(ServiceProviderFactory?.Invoke(), request, cancellationToken);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
