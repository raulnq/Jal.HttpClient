using System.Text;

namespace Jal.HttpClient.Model
{
    public abstract class HttpRequestSimpleDataContent : HttpRequestContent
    {
        public HttpContentDisposition Disposition { get; set; }

        protected HttpRequestSimpleDataContent()
        {
            Disposition = new HttpContentDisposition();
        }

        public Encoding GetEncoding()
        {
            return !string.IsNullOrEmpty(CharacterSet) ? Encoding.GetEncoding(CharacterSet.Replace("charset=", "")) : GetDefaultEncoding();
        }
    }
}