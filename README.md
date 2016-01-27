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
