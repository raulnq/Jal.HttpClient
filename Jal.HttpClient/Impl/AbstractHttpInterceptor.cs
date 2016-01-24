using System.Net;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public abstract class AbstractHttpInterceptor : IHttpInterceptor
    {
        public static NullHttpInterceptor Instance = new NullHttpInterceptor();

        public virtual void OnEntry(HttpRequest request)
        {

        }

        public virtual void OnExit(HttpResponse response, HttpRequest request)
        {

        }

        public virtual void OnSuccess(HttpResponse response, HttpRequest request)
        {

        }

        public virtual void OnError(HttpResponse response, HttpRequest request, WebException wex)
        {

        }
    }
}