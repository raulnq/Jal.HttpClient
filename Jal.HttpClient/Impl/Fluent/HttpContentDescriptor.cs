using System.Threading.Tasks;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl.Fluent
{
    public class HttpContentDescriptor : IHttpContentTypeDescriptor
    {
        private readonly HttpContent _httpcontent;

        private readonly HttpContext _httpcontext;

        public HttpContentDescriptor(HttpContent httpcontent, HttpContext httpcontext)
        {
            _httpcontent = httpcontent;
            _httpcontext = httpcontext;
        }

        public IHttpContentTypeDescriptor WithCharacterSet(string characterset)
        {
            _httpcontent.CharacterSet = characterset;
            return this;
        }


        public IHttpContentTypeDescriptor WithDisposition(string name, string filename = "")
        {
            _httpcontent.Disposition.Name = name;
            _httpcontent.Disposition.FileName = filename;
            return this;
        }

        public IHttpContentTypeDescriptor WithContentType(string contenttype)
        {
            _httpcontent.ContentType = contenttype;
            return this;
        }

        public HttpResponse Send
        {
            get
            {
                if (_httpcontext.QueryParemeterDescriptorAction != null)
                {
                    var queryParemeterDescriptor = new HttpQueryParameterDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.QueryParemeterDescriptorAction(queryParemeterDescriptor);
                }

                if (_httpcontext.HeaderDescriptorAction != null)
                {
                    var headerDescriptor = new HttpHeaderDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.HeaderDescriptorAction(headerDescriptor);
                }

                return _httpcontext.HttpHandler.Send(_httpcontext.HttpRequest);
            }

        }

        public Task<HttpResponse> SendAsync
        {
            get
            {
                if (_httpcontext.QueryParemeterDescriptorAction != null)
                {
                    var queryParemeterDescriptor = new HttpQueryParameterDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.QueryParemeterDescriptorAction(queryParemeterDescriptor);
                }

                if (_httpcontext.HeaderDescriptorAction != null)
                {
                    var headerDescriptor = new HttpHeaderDescriptor(_httpcontext.HttpRequest);
                    _httpcontext.HeaderDescriptorAction(headerDescriptor);
                }

                return _httpcontext.HttpHandler.SendAsync(_httpcontext.HttpRequest);
            }
        }

    }
}