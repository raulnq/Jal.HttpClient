# Jal.HttpClient
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

### Castle Windsor
```csharp
var container = new WindsorContainer();

container.AddHttpClient();

var client = container.Resolve<IHttpFluentHandler>();
```
### LightInject
```csharp
var container = new ServiceContainer();

container.AddHttpClient();

var client = container.GetInstance<IHttpFluentHandler>();
```
### Microsoft.Extensions.DependencyInjection
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
### Serilog
```csharp
container.AddSerilogForHttpClient();
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseSerilog()).SendAsync())
{

}
```
### Polly
```csharp
container.AddPollyForHttpClient();
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseTimeout(5)).SendAsync())
{

}

using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.WithMiddleware(x => x.OnConditionRetry(3, y => y.Message?.StatusCode != HttpStatusCode.OK)).SendAsync())
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
### Application Insights
```csharp
container.AddApplicationInsightsForHttpClient("appname");
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseApplicationInsights()).SendAsync())
{

}
```
### Common Logging
```csharp
container.AddCommonLoggingForHttpClient();
...
using (var response = await _sut.Get("http://httpbin.org/ip")
    .WithMiddleware(x=>x.UseCommonLogging()).SendAsync())
{

}
```