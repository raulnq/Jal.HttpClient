# Jal.HttpClient
Just another library to send HTTP requests and receve HTTP responses from a resource identified by a URI

## How to use?

### Default Implementation

    var httpclient = HttpHandler.Builder.Create;

### Castle Windsor Integration

Setup the Castle Windsor container

    var container = new WindsorContainer();
	
Install the installer class included in the Jal.HttpClient.Installer library

    container.Install(new HttpClientInstaller());
				
Resolve an instance of the IHttpHandler class

    var httpclient = container.Resolve<IHttpHandler>();

Send a request to https://github.com/raulnq

    var response = httpclient.Send(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));

### LightInject Integration

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

Sned data

    var response = httpfluenthandler.Get("https://github.com/raulnq").Send;

Post Json data

    var response = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Send;
    
Post Xml data

    var response = httpfluenthandler.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").Send;
    
Post Form url encoded data

    var response = httpfluenthandler.Post("http://httpbin.org/post").FormUrlEncoded(@"message=Hello%World!!").Send()
    
Get Async data

    var task = httpfluenthandler.Get("http://httpbin.org//get").SendAsync;

    var response = task.Result;
    
Post UTF-16 character set data

    var response = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf16().Send;

Post UTF-16 character set data

    var response = httpfluenthandler.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf8().Send;

Setup timeout

    var response = httpclientbuilder.Get("http://httpbin.org/delay/5").WithTimeout(10).Send;

Send query parameters

    var r = httpclientbuilder.Get("http://httpbin.org/ip").WithQueryParameters( x=> { x.Add("x", "x"); x.Add("y","y"); }).Send;

Send headers

    var r = httpclientbuilder.Get("http://httpbin.org/ip").WithHeaders(x=> { x.Add("x", "x"); x.Add("y","y"); }).Send;