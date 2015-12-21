using System.Net;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IWebResponseToHttpResponseConverter
    {
        HttpResponse Convert(WebResponse webResponse);

        HttpResponse Convert(WebException webException);
    }
}
