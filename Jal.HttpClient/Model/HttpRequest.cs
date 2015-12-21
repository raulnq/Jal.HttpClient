using System.Collections.Generic;
using System.Linq;

namespace Jal.HttpClient.Model
{
    public class HttpRequest
    {
        public HttpContentType HttpContentType { get; set; }

        public string Body { get; set; }

        public int TimeoutInSeconds { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string Url { get; set; }

        public List<HttpHeader> Headers { get; set; }

        public List<HttpQueryParameter> QueryParameters { get; set; }

        public void AddHeader(string name, string value)
        {
            var item = Headers.FirstOrDefault(x => x.Name == name);
            if (item!=null)
            {
                Headers.Remove(item);
            }
            Headers.Add(new HttpHeader() { Value = value, Name = name });
        }

        public void AddQueryParameter(string name, string value)
        {
            QueryParameters.Add(new HttpQueryParameter() { Name = name, Value = value });
        }

        public HttpRequest(string url, HttpMethod httpMethod, HttpContentType httpContentType)
        {
            Url = url;
            TimeoutInSeconds = 20;
            QueryParameters = new List<HttpQueryParameter>();
            Headers = new List<HttpHeader>();
            HttpMethod = httpMethod;
            HttpContentType = httpContentType;
        }
    }
}