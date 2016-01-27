using System.IO;
using System.Net;
using System.Text;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class WebResponseToHttpResponseConverter : IWebResponseToHttpResponseConverter
    {
        public HttpResponse Convert(WebResponse webResponse)
        {
            var httpResponse = new HttpResponse()
            {
                HttpStatusCode = HttpStatusCode.OK,
            };

            var response = (HttpWebResponse) webResponse;

            ReadContent(response, httpResponse);

            return httpResponse;
        }

        public HttpResponse Convert(WebException webException)
        {
            var httpResponse = new HttpResponse()
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,

                WebException = webException
            };

            if (webException.Status == WebExceptionStatus.ProtocolError)
            {
                ReadContent((HttpWebResponse) webException.Response, httpResponse);
            }
            return httpResponse;
        }

        private void ReadContent(HttpWebResponse response, HttpResponse httpResponse)
        {
            using (var responseStream = response.GetResponseStream())
            {
                httpResponse.Url = response.ResponseUri.ToString();

                httpResponse.HttpStatusCode = response.StatusCode;

                httpResponse.ContentType = response.ContentType;

                httpResponse.ContentLength = response.ContentLength;

                foreach (var headerName in response.Headers.AllKeys)
                {
                    var headerValue = response.Headers[headerName];

                    httpResponse.Headers.Add(new HttpHeader {Name = headerName, Value = headerValue});
                }

                if (responseStream != null)
                {
                    var encoding = string.IsNullOrEmpty(response.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(response.CharacterSet);

                    using (var ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);

                        httpResponse.Bytes = ms.ToArray();

                        httpResponse.Content = encoding.GetString(httpResponse.Bytes);
                    }
                }
            }
        }
    }
}
