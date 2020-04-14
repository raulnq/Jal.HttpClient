using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{
    public interface IHttpClientBuilder
    {
        IHttpClientBuilder Add<TImplementation>() where TImplementation: class, IAsyncMiddleware<HttpContext>;
    }
}