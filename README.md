# Jal.HttpClient
Just another library to send HTTP requests and receve HTTP responses from a resource identified by a URI

## How to use?

### Castle Windsor Implementation

Setup the Castle Windsor container
```c++
var container = new WindsorContainer();
```	
Install the installer class included in the Jal.HttpClient.Installer library
```c++
container.Install(new HttpClientInstaller());
```			
Resolve an instance of the IHttpHandler class
```c++
var httpclient = container.Resolve<IHttpHandler>();
```
Send a request to https://github.com/raulnq
```c++
var response = await httpclient.SendAsync(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));
```
### LightInject Implementation

Setup the LightInject container
```c++
var container = new ServiceContainer();
```
Install the installer class included in the Jal.HttpClient.LightInject.Installer library
```c++
container.RegisterHttpClient();
```			
Resolve an instance of the IHttpHandler class
```c++
var httpclient = container.GetInstance<IHttpHandler>();
```
Send a request to https://github.com/raulnq
```c++
var response = httpclient.SendAsync(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));
```
## Fluent API
 
Resolve an instance of IHttpFluentHandler.
```c++
var httpfluenthandler = container.Resolve<IHttpFluentHandler>();
```
Get data
```c++
using (var r = await httpfluenthandler.Get("https://github.com/raulnq").SendAsync())
{
    var content = r.Message.Content.Read();
}
```
Post Json data
```c++
using (var r = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").SendAsync())
{

}
```
Post Xml data
```c++
using (var r = httpfluenthandler.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").SendAsync())
{

}
```
Post Form url encoded data
```c++
using (var r = await httpfluenthandler.Post("http://httpbin.org/post").FormUrlEncoded(new [] {new KeyValuePair<string, string>("message", "Hello World") }).SendAsync())
{

}
```
Get Async data
```c++
using (var r = await _sut.Get("http://httpbin.org/ip").SendAsync())
{
	var content = await r.Message.Content.ReadAsStringAsync();
}
```
Send query parameters
```c++
using (var r = await httpclientbuilder.Get("http://httpbin.org/ip").WithQueryParameters( x=> { x.Add("x", "x"); x.Add("y","y"); }).SendAsync())
{

}
```
Send headers
```c++
using (var r = await httpclientbuilder.Get("http://httpbin.org/ip").WithHeaders(x=> { x.Add("x", "x"); x.Add("y","y"); }).SendAsync())
{

}
```