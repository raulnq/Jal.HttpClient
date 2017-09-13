# Jal.HttpClient
Just another library to send HTTP requests and receve HTTP responses from a resource identified by a URI

## How to use?

### Default Implementation

    var httpclient = HttpHandler.Builder.Create;

### Castle Windsor Implementation

Setup the Castle Windsor container

    var container = new WindsorContainer();
	
Install the installer class included in the Jal.HttpClient.Installer library

    container.Install(new HttpClientInstaller());
				
Resolve an instance of the IHttpHandler class

    var httpclient = container.Resolve<IHttpHandler>();

Send a request to https://github.com/raulnq

    var response = httpclient.Send(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));

### LightInject Implementation

Setup the LightInject container

    var container = new ServiceContainer();
	
Install the installer class included in the Jal.HttpClient.LightInject.Installer library

    container.RegisterHttpClient();
				
Resolve an instance of the IHttpHandler class

    var httpclient = container.GetInstance<IHttpHandler>();
	
Send a request to https://github.com/raulnq

    var response = httpclient.Send(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));
    
## Fluent API
 
Resolve an instance of IHttpFluentHandler.

    var httpclientfluent = HttpFluentHandler.Builder.UseHttpHandler(httpclient).Create;

Or

    var httpfluenthandler = container.Resolve<IHttpFluentHandler>();

Or

    var httpfluenthandler = container.GetInstance<IHttpFluentHandler>();

Get data

    using (var response = httpfluenthandler.Get("https://github.com/raulnq").Send)
    {
        var content = response.Content.Read();
    }

Post Json data

    using (var response = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Send)
    {
        var code = response.HttpStatusCode;
    }
    
Post Xml data

    using (var response = httpfluenthandler.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").Send)
    {
        
    }
    
Post Form url encoded data

    using (var response = httpfluenthandler.Post("http://httpbin.org/post").FormUrlEncoded(@"message=Hello%World!!").Send())
    {

    }
    
Get Async data

    using (var response = await _sut.Get("http://httpbin.org/ip").SendAsync)
    {

    }
    
Post UTF-16 character set data

    using (var response = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf16().Send)
    {

    }

Post UTF-16 character set data

    using (var response = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf8().Send)
    {

    }

Setup timeout

    using (var response = httpclientbuilder.Get("http://httpbin.org/delay/5").WithTimeout(10).Send)
    {
        
    }

Send query parameters

    using (var r = httpclientbuilder.Get("http://httpbin.org/ip").WithQueryParameters( x=> { x.Add("x", "x"); x.Add("y","y"); }).Send)
    {

    }

Send headers

    using (var r = httpclientbuilder.Get("http://httpbin.org/ip").WithHeaders(x=> { x.Add("x", "x"); x.Add("y","y"); }).Send)
    {

    }