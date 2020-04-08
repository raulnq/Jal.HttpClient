using System;
using System.Net;
using System.Threading.Tasks;
using Jal.ChainOfResponsability;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.HttpClient.ApplicationInsights
{
    public class ApplicationInsightsMiddelware : IAsyncMiddleware<HttpContext>
    {
        private readonly TelemetryClient _client;

        private readonly string _applicationname;

        public ApplicationInsightsMiddelware(TelemetryClient client, string applicationname)
        {
            _client = client;
            _applicationname = applicationname;
        }

        public async Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            var telemetry = new DependencyTelemetry()
            {
                Name = context.Data.Request.Message.RequestUri.AbsolutePath,

                Id = context.Data.Request.Tracing.RequestId,

                Timestamp = DateTime.UtcNow,

                Target = context.Data.Request.Message.RequestUri.Host,

                Data = context.Data.Request.Message.RequestUri.ToString(),

                Type = "HTTP",
            };

            if (!string.IsNullOrWhiteSpace(context.Data.Request.Tracing.OperationId))
            {
                telemetry.Context.Operation.Id = context.Data.Request.Tracing.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Request.Tracing.ParentId))
            {
                telemetry.Context.Operation.ParentId = context.Data.Request.Tracing.ParentId;
            }

            if (!string.IsNullOrWhiteSpace(_applicationname))
            {
                telemetry.Context.Cloud.RoleName = _applicationname;
            }

            HttpResponse response = null;

            try
            {

                await next(context);

                response = context.Data.Response;

                if (response.Message?.StatusCode == HttpStatusCode.OK)
                {
                    telemetry.Success = true;

                    telemetry.ResultCode = "200";
                }
                else
                {
                    telemetry.Success = false;

                    if (response.Message?.StatusCode != null)
                    {
                        telemetry.ResultCode = ((int)response.Message.StatusCode).ToString();
                    }
                    else
                    {
                        telemetry.ResultCode = "500";
                    }
                }
            }
            catch (Exception exception)
            {
                telemetry.Success = false;

                telemetry.ResultCode = "500";

                var telemetryexception = new ExceptionTelemetry(exception);

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Tracing.OperationId))
                {
                    telemetryexception.Context.Operation.Id = context.Data.Request.Tracing.OperationId;
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Tracing.ParentId))
                {
                    telemetryexception.Context.Operation.ParentId = context.Data.Request.Tracing.ParentId;
                }

                if (!string.IsNullOrWhiteSpace(_applicationname))
                {
                    telemetryexception.Context.Cloud.RoleName = _applicationname;
                }

                _client.TrackException(telemetryexception);

                throw;
            }
            finally
            {
                if (response != null)
                {
                    telemetry.Duration = TimeSpan.FromMilliseconds(response.Duration);
                }

                _client.TrackDependency(telemetry);
            }
        }
    }
}
