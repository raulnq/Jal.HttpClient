using System.IO;
using System.Net;
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
                HttpExceptionStatus = webException.Status,

                Exception = webException
            };

            if (webException.Status == WebExceptionStatus.ProtocolError)
            {
                ReadContent((HttpWebResponse) webException.Response, httpResponse);
            }
            return httpResponse;
        }

        private void ReadContent(HttpWebResponse response, HttpResponse httpResponse)
        {
            httpResponse.Uri = response.ResponseUri;

            httpResponse.HttpStatusCode = response.StatusCode;

            httpResponse.Content.ContentType = response.ContentType;

            httpResponse.Content.ContentLength = response.ContentLength;

            httpResponse.Content.CharacterSet = response.CharacterSet;

            foreach (var headerName in response.Headers.AllKeys)
            {
                var headerValue = response.Headers[headerName];

                httpResponse.Headers.Add(new HttpHeader(headerName, headerValue));
            }

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    httpResponse.Content.Stream = new MemoryStream();
                    responseStream.CopyTo(httpResponse.Content.Stream);
                }
            }
        }
    }
}
