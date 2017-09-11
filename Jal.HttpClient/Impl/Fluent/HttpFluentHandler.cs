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

        public IHttpDescriptor Get(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Get);
        }

        public IHttpDescriptor Post(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Post);
        }

        public IHttpDescriptor Put(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Put);
        }

        public IHttpDescriptor Head(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Head);
        }

        public IHttpDescriptor Delete(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Delete);
        }

        public IHttpDescriptor Patch(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Patch);
        }

        public IHttpDescriptor Options(string url)
        {
            return new HttpDescriptor(url, Handler, HttpMethod.Options);
        }
    }


}
