using System;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface.Fluent
{
    public interface IHttpHandlerDescriptor
    {
        IHttpHandlerDescriptor WithDecompressionMethods(DecompressionMethods decompressionMethods);

        IHttpHandlerDescriptor WithContentType(string contentType);

        IHttpHandlerDescriptor WithAcceptedType(string acceptedType);

        IHttpHandlerDescriptor WithCharacterSet(string characterSet);

        IHttpHandlerDescriptor WithTimeout(int timeout);

        IHttpHandlerDescriptor WithContent(string content);

        IHttpHandlerDescriptor WithHeaders(Action<IHeaderDescriptor> headerDescriptorAction);

        IHttpHandlerDescriptor WithQueryParameters(Action<IQueryParameterDescriptor> queryParemeterDescriptorAction);

        HttpResponse Send { get; }

        Task<HttpResponse> SendAsync { get; }

    }
}