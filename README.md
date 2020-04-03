# Codevos.Net.HttpClient

Adds System.Text.Json HttpClient extension methods and provides a universal way to register HTTP clients.

This library implements:
- an extension method named 'AddHttpClients' for the IServiceCollection.
- an extension method named 'ForeachRequest' for the IHttpClientBuilder.
- extension methods for System.Net.Http.HttpClient to GET / POST / PUT / PATCH as JSON using System.Text.Json.JsonSerializerOptions
- extensions methods for System.Net.Http.HttpContent to deserialize from JSON using System.Text.Json.JsonSerializerOptions
- a class HttpClientDelegatingHandler to modify the HTTP requests before send.

### Configure HTTP clients example usage in .NET core web application

Define settings in the appsettings.json

```json
  "HttpClients": {
    "IOtherService1": "https://otherservice1.com/service",
    "IOtherService2": "https://otherservice2.com/service"
  }
```

Register the HTTP clients in Startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Get HTTP clients configurations from appsettings.json
    var httpClientsDictionary = Configuration.GetSection("HttpClients").Get<Dictionary<string, string>>();

    // Add HTTP clients
    services
        .AddHttpClients(
            httpClientsDictionary,
            (serviceName, baseUrl, httpClientBuilder) =>
            {
                httpClientBuilder
                    // For each HTTP request
                    .ForeachRequest((serviceProvider, request, cancellationToken) =>
                    {
                        // add generic code here
                    });

                    switch (serviceName)
                    {
                        case nameof(IOtherService1):
                            // Add typed client using Refit RestService.For
                            httpClientBuilder.AddTypedClient(RestService.For<IOtherService1>);
                            break;
                    }
            }
        );
}
```