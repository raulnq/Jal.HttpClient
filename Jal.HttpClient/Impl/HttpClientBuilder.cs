using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private readonly IChainOfResponsabilityBuilder _builder;

        public HttpClientBuilder(IChainOfResponsabilityBuilder builder)
        {
            _builder = builder;
        }

        public IHttpClientBuilder Add<TImplementation>() where TImplementation : class, IAsyncMiddleware<HttpContext>
        {
            _builder.AddAsyncMiddleware<TImplementation, HttpContext>();

            return this;
        }
    }
}
