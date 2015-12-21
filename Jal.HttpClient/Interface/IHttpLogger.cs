using Jal.HttpClient.Model;

namespace Jal.HttpClient.Interface
{
    public interface IHttpLogger
    {
        void Log(HttpRequest request);

        void Log(HttpResponse response);
    }
}
