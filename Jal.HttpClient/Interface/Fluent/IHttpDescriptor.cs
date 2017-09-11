using System;
using System.Net;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpDescriptor : IHttpContentDescriptor, IHttpSenderDescriptor
    {
        IHttpDescriptor WithDecompressionMethods(DecompressionMethods decompressionmethods);

        IHttpDescriptor WithTimeout(int timeout);

        IHttpDescriptor WithAllowWriteStreamBuffering(bool allowwritestreambuffering);

        IHttpDescriptor WithAcceptedType(string acceptedtype);

        IHttpDescriptor WithHeaders(Action<IHttpHeaderDescriptor> action);

        IHttpDescriptor WithQueryParameters(Action<IHttpQueryParameterDescriptor> action);
    }
}