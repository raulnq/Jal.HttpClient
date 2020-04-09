# Jal.HttpClient [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.svg)](https://www.nuget.org/packages/Jal.HttpClient)
Just another library to wrap the HttpClient class

## How to use?

#### Get
```csharp
using (var response = await client.Get("http://httpbin.org/ip").SendAsync())
{
    var content = await response.Message.Content.ReadAsStringAsync();
}

using (var response = await client.Get("http://httpbin.org/ip")
    .WithQueryParameters( x=> { x.Add("x", "x"); x.Add("y","y"); }).SendAsync())
{
    var content = await response.Message.Content.ReadAsStringAsync();
}

using (var response = await httpclientbuilder.Get("http://httpbin.org/ip")
    .WithHeaders(x=> { x.Add("x", "x"); x.Add("y","y"); }).SendAsync())
{
    var content = await response.Message.Content.ReadAsStringAsync();
}
```
#### Post
```csharp
using (var response = await client.Post("http://httpbin.org/post")
    .Json(@"{""message"":""Hello World!!""}").SendAsync())
{

}

using (var response = await client.Post("http://httpbin.org/post")
    .Xml(@"<message>Hello World!!</message>").SendAsync())
{

}

using (var response = await client.Post("http://httpbin.org/post")
    .FormUrlEncoded(new [] {new KeyValuePair<string, string>("message", "Hello World") }).SendAsync())
{

}
```
#### Delete
```csharp
using (var response = await client.Delete("http://httpbin.org/delete").SendAsync())
{

}
```
#### Patch, Put, Options and Head
## IHttpFluentHandler interface building

### Castle Windsor [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.Installer.svg)](https://www.nuget.org/packages/Jal.HttpClient.Installer)
```csharp
var container = new WindsorContainer();

container.AddHttpClient();

var client = container.Resolve<IHttpFluentHandler>();
```
### LightInject [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.LightInject.Installer.svg)](https://www.nuget.org/packages/Jal.HttpClient.LightInject.Installer)
```csharp
var container = new ServiceContainer();

container.AddHttpClient();

var client = container.GetInstance<IHttpFluentHandler>();
```
### Microsoft.Extensions.DependencyInjection [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.Microsoft.Extensions.DependencyInjection.Installer.svg)](https://www.nuget.org/packages/Jal.HttpClient.Microsoft.Extensions.DependencyInjection.Installer)
```csharp
var container = new ServiceCollection();

container.AddHttpClient();

var provider = container.BuildServiceProvider();

var client = provider.GetService<IHttpFluentHandler>();
```
## Middlewares
```csharp
using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x =>
{
    x.AddTracingInformation();
    x.AuthorizedByToken("token", "value");
    x.UseMemoryCache(30, y => y.Message.RequestUri.AbsoluteUri, z => z.Message.StatusCode == HttpStatusCode.OK);
}).SendAsync())
{
    var content = await response.Message.Content.ReadAsStringAsync();
}
```
### Serilog [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.Serilog.svg)](https://www.nuget.org/packages/Jal.HttpClient.Serilog)
```csharp
container.AddSerilogForHttpClient();
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseSerilog()).SendAsync())
{

}
```
### Polly [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.Polly.svg)](https://www.nuget.org/packages/Jal.HttpClient.Polly)
```csharp
container.AddPollyForHttpClient();
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseTimeout(5)).SendAsync())
{

}

using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x => x.OnConditionRetry(3, y => y.Message?.StatusCode != HttpStatusCode.OK)).SendAsync())
{

}

var policy = Policy
.HandleResult<HttpResponse>(r => r.Message?.StatusCode!= HttpStatusCode.OK )
.CircuitBreakerAsync(2, TimeSpan.FromSeconds(10));

using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x => x.UseCircuitBreaker(policy)).SendAsync())
{
    response.Message.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
}
```
### Application Insights [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.ApplicationInsights.svg)](https://www.nuget.org/packages/Jal.HttpClient.ApplicationInsights)
```csharp
container.AddApplicationInsightsForHttpClient("appname");
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseApplicationInsights()).SendAsync())
{

}
```
### Common Logging [![NuGet](https://img.shields.io/nuget/v/Jal.HttpClient.Common.Logging.svg)](https://www.nuget.org/packages/Jal.HttpClient.Common.Logging)
```csharp
container.AddCommonLoggingForHttpClient();
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseCommonLogging()).SendAsync())
{

}
```