using System;
using System.Threading;

namespace Jal.HttpClient
{
    public interface IHttpFluentHandler
    {
        IHttpDescriptor Get(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));

        IHttpDescriptor Post(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));

        IHttpDescriptor Put(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));

        IHttpDescriptor Head(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));

        IHttpDescriptor Delete(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));

        IHttpDescriptor Patch(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));

        IHttpDescriptor Options(string url, System.Net.Http.HttpClient httpclient = null, Func<System.Net.Http.HttpClient> factory = null, CancellationToken cancellationtoken = default(CancellationToken));
    }
}