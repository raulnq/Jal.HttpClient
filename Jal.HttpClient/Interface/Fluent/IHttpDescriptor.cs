using System;
using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpDescriptor : IHttpContentDescriptor, IHttpSenderDescriptor
    {
        IHttpDescriptor WithDecompression(DecompressionMethods decompression);

        IHttpDescriptor WithTimeout(int timeout);

        IHttpDescriptor WithAllowWriteStreamBuffering(bool allowwritestreambuffering);

        IHttpDescriptor WithAcceptedType(string acceptedtype);

        IHttpDescriptor WithHeaders(Action<IHttpHeaderDescriptor> action);

        IHttpDescriptor AuthorizedBy(Action<HttpRequest> authenticator);

        IHttpDescriptor WithQueryParameters(Action<IHttpQueryParameterDescriptor> action);
    }
}