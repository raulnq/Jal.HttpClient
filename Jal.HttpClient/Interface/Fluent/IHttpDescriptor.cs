using System;
using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpDescriptor : IHttpContentDescriptor, IHttpSenderDescriptor
    {
        IHttpDescriptor WithDecompression(DecompressionMethods decompression);

        IHttpDescriptor WithTimeout(int timeout);

        IHttpDescriptor WithIdentity(HttpIdentity identity);

        IHttpDescriptor WithAllowWriteStreamBuffering(bool allowwritestreambuffering);

        IHttpDescriptor WithAcceptedType(string acceptedtype);

        IHttpDescriptor WithHeaders(Action<IHttpHeaderDescriptor> action);

        IHttpDescriptor WithMiddleware(Action<IHttpMiddlewareDescriptor> action);

        IHttpDescriptor WithQueryParameters(Action<IHttpQueryParameterDescriptor> action);
    }
}