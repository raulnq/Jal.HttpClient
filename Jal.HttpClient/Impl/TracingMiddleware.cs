using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{
    public class TracingMiddleware : IAsyncMiddleware<HttpContext>
    {
        public const string REQUESTID_HEADER_NAME_KEY = "requestidheadername";

        public const string OPERATIONID_HEADER_NAME_KEY = "operationidheadername";

        public const string PARENTID_HEADER_NAME_KEY = "parentidheadername";

        public Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            if(context.Data.Request.Tracing!=null)
            {
                if(context.Data.Request.Context.ContainsKey(REQUESTID_HEADER_NAME_KEY))
                {
                    var name = context.Data.Request.Context[REQUESTID_HEADER_NAME_KEY] as string;

                    context.Data.Request.Message.Headers.Add(name, context.Data.Request.Tracing.RequestId);
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Tracing.OperationId) && context.Data.Request.Context.ContainsKey(OPERATIONID_HEADER_NAME_KEY))
                {
                    var name = context.Data.Request.Context[OPERATIONID_HEADER_NAME_KEY] as string;

                    context.Data.Request.Message.Headers.Add(name, context.Data.Request.Tracing.OperationId);
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Tracing.ParentId) && context.Data.Request.Context.ContainsKey(PARENTID_HEADER_NAME_KEY))
                {
                    var name = context.Data.Request.Context[PARENTID_HEADER_NAME_KEY] as string;

                    context.Data.Request.Message.Headers.Add(name, context.Data.Request.Tracing.ParentId);
                }
            }

            return next(context);
        }
    }
}
