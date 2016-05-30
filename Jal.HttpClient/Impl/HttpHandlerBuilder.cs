using Jal.HttpClient.Fluent;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpHandlerBuilder : IHttpHandlerBuilder
    {
        public static IHttpHandlerBuilder Current;

        public static IHttpHandlerBuilderStartFluentBuilder Builder
        {
            get
            {
                return new HttpHandlerBuilderFluentBuilder();
            }
        }

        public IHttpHandler Handler { get; set; }

        public HttpHandlerBuilder(IHttpHandler httpHandler)
        {
            Handler = httpHandler;
        }

        public HttpHandlerDescriptor Get(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Get);
        }

        public HttpHandlerDescriptor Post(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Post);
        }

        public HttpHandlerDescriptor Put(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Put);
        }

        public HttpHandlerDescriptor Head(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Head);
        }

        public HttpHandlerDescriptor Delete(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Delete);
        }

        public HttpHandlerDescriptor Patch(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Patch);
        }

        public HttpHandlerDescriptor Options(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Options);
        }
    }


}
