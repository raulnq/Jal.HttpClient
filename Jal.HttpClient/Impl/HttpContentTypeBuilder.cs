using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
  public class HttpContentTypeBuilder : IHttpContentTypeBuilder
    {
        public string Build(string httpContentType, string httpCharacterSet)
        {
            return string.Format("{0}; {1}", httpContentType, httpCharacterSet);
        }

 

      
    }
}
