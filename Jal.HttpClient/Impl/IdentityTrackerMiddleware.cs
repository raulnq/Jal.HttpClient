using System;
using System.Threading.Tasks;
using Jal.HttpClient.Model;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;

namespace Jal.HttpClient.Impl
{
    public class IdentityTrackerMiddleware : IMiddlewareAsync<HttpWrapper>
    {
        public Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
        {
            if(context.Data.Request.Identity!=null)
            {
                if(context.Data.Request.Context.ContainsKey("idheadername"))
                {
                    var name = context.Data.Request.Context["idheadername"] as string;

                    context.Data.Request.Message.Headers.Add(name, context.Data.Request.Identity.Id);
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.OperationId) && context.Data.Request.Context.ContainsKey("operationidheadername"))
                {
                    var name = context.Data.Request.Context["operationidheadername"] as string;

                    context.Data.Request.Message.Headers.Add(name, context.Data.Request.Identity.OperationId);
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.ParentId) && context.Data.Request.Context.ContainsKey("parentidheadername"))
                {
                    var name = context.Data.Request.Context["parentidheadername"] as string;

                    context.Data.Request.Message.Headers.Add(name, context.Data.Request.Identity.ParentId);
                }
            }

            return next(context);
        }
    }
}
