using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;

namespace Jal.HttpClient.Impl
{
    public class NullHttpLogger : IHttpLogger
    {
        public static NullHttpLogger Instance = new NullHttpLogger();

        public void Log(HttpRequest request)
        {
        }

        public void Log(HttpResponse response)
        {
        }
    }
}
