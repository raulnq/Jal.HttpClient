using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpContentTypeMapper : IHttpContentTypeMapper
    {
        public string Map(HttpContentType httpContentType)
        {
            switch (httpContentType)
            {
                case HttpContentType.Form: return "application/x-www-form-urlencoded; charset=UTF-8";
                case HttpContentType.Json: return "application/json; charset=UTF-8";
                case HttpContentType.Xml: return "text/xml; charset=UTF-8";
                default:
                    return "text/xml; charset=UTF-8";
            }
        }
    }
}
