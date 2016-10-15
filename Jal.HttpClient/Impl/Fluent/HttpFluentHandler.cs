using Jal.HttpClient.Interface;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpFluentHandler : IHttpFluentHandler
    {
        public static IHttpFluentHandler Current;

        public static IHttpFluentHandlerStartBuilder Builder => new HttpFluentHandlerBuilder();

        public IHttpHandler Handler { get; set; }

        public HttpFluentHandler(IHttpHandler httpHandler)
        {
            Handler = httpHandler;
        }

        public IHttpHandlerDescriptor Get(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Get);
        }

        public IHttpHandlerDescriptor Post(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Post);
        }

        public IHttpHandlerDescriptor Put(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Put);
        }

        public IHttpHandlerDescriptor Head(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Head);
        }

        public IHttpHandlerDescriptor Delete(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Delete);
        }

        public IHttpHandlerDescriptor Patch(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Patch);
        }

        public IHttpHandlerDescriptor Options(string url)
        {
            return new HttpHandlerDescriptor(url, Handler, HttpMethod.Options);
        }
    }


}
