using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class HttpContentTypeMapper : IHttpContentTypeMapper
    {
        public string Map(HttpContentType httpContentType, HttpCharacterSet httpCharacterSet)
        {
            var charset = GetCharset(httpCharacterSet);

            var contentType = GetContentType(httpContentType);

            return string.Format("{0}; {1}", contentType, charset);
        }

        private string GetContentType(HttpContentType httpContentType)
        {
            string contentType;

            switch (httpContentType)
            {
                case HttpContentType.Form:
                    contentType = "application/x-www-form-urlencoded";
                    break;
                case HttpContentType.Json:
                    contentType = "application/json";
                    break;
                case HttpContentType.Xml:
                    contentType = "text/xml";
                    break;
                default:
                    contentType = "text/xml";
                    break;
            }
            return contentType;
        }

        private string GetCharset(HttpCharacterSet httpCharacterSet)
        {
            string charset;

            switch (httpCharacterSet)
            {
                case HttpCharacterSet.Utf8:
                    charset = "charset=UTF-8";
                    break;
                case HttpCharacterSet.Utf16:
                    charset = "charset=UTF-16";
                    break;
                case HttpCharacterSet.Utf7:
                    charset = "charset=UTF-7";
                    break;
                case HttpCharacterSet.Utf32:
                    charset = "charset=UTF-32";
                    break;
                default:
                    charset = "charset=UTF-8";
                    break;
            }
            return charset;
        }
    }
}
