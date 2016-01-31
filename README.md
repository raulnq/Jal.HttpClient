# Jal.HttpClient
Just another library to send HTTP requests and receve HTTP responses from a resource identified by a URI

## How to use?

Setup the Castle Windsor container

    var container = new WindsorContainer();
	
Install the installer class included in the Jal.HttpClient.Installer library

    container.Install(new HttpClientInstaller());
				
Resolve an instance of the IHttpHandler class

    var httpclient = container.Resolve<IHttpHandler>();
	
Send a request to https://github.com/raulnq

    var response = httpclient.Send(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));
    
## Fluent API
 
    var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

    var response = httpclientbuilder.Get("https://github.com/raulnq").Send();

Post Json data

    var response = httpclientbuilder.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf8().Send();
    
Post Xml data

    var response = httpclientbuilder.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").Utf8().Send();
    
Post Form url encoded data

    httpclientbuilder.Post("http://httpbin.org/post").FormUrlEncoded(@"message=Hello%World!!").Utf8().Send()
    
Get Async data

    var task = httpclientbuilder.Get("http://httpbin.org//get").SendAsync();

    var response = task.Result;
    
Post UTF-16 character set data

    var response = httpclientbuilder.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf16().Send();
