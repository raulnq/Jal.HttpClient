using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpMethodMapper:  IHttpMethodMapper
    {
        public string Map(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                    case HttpMethod.Delete: return "DELETE";
                    case HttpMethod.Get:return "GET";
                    case HttpMethod.Head: return "HEAD";
                    case HttpMethod.Merge: return "MERGE";
                    case HttpMethod.Options: return "OPTIONS";
                    case HttpMethod.Patch: return "PATCH";
                    case HttpMethod.Post: return "POST";
                    case HttpMethod.Put: return "PUT";
                default:
                    return "GET";
            }
        }
    }
}
