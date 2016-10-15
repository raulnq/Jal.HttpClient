using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class QueryParameterDescriptor : IQueryParameterDescriptor
    {
        private readonly HttpRequest _httpRequest;

        public QueryParameterDescriptor(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public void Add(string name, string value)
        {
            _httpRequest.QueryParameters.Add(new HttpQueryParameter() { Name = name, Value = value });
        }
    }
}