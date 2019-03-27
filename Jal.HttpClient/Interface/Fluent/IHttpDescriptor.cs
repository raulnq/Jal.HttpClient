using System;
using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpDescriptor : IHttpContentDescriptor, IHttpSenderDescriptor
    {
        IHttpDescriptor WithIdentity(HttpIdentity identity);

        IHttpDescriptor WithAcceptedType(string acceptedtype);

        IHttpDescriptor WithHeaders(Action<IHttpHeaderDescriptor> action);

        IHttpDescriptor WithMiddleware(Action<IHttpMiddlewareDescriptor> action);

        IHttpDescriptor WithQueryParameters(Action<IHttpQueryParameterDescriptor> action);
    }
}