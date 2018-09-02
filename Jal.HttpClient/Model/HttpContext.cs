using System;

namespace Jal.HttpClient.Model
{
    public class HttpContext
    {
        public int Index { get; set; }

        public Type[] MiddlewareTypes { get; set; }
    }
}