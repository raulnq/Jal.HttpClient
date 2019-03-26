namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpFluentHandler
    {
        IHttpDescriptor Get(string url, System.Net.Http.HttpClient httpclient = null);

        IHttpDescriptor Post(string url, System.Net.Http.HttpClient httpclient = null);

        IHttpDescriptor Put(string url, System.Net.Http.HttpClient httpclient = null);

        IHttpDescriptor Head(string url, System.Net.Http.HttpClient httpclient = null);

        IHttpDescriptor Delete(string url, System.Net.Http.HttpClient httpclient = null);

        IHttpDescriptor Patch(string url, System.Net.Http.HttpClient httpclient = null);

        IHttpDescriptor Options(string url, System.Net.Http.HttpClient httpclient = null);

    }
}