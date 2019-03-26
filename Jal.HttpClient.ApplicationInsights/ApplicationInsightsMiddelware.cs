using System;
using System.Net;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.HttpClient.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.HttpClient.ApplicationInsights
{
    public class ApplicationInsightsMiddelware : IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>
    {
        private readonly TelemetryClient _client;

        private readonly string _applicationname;

        public ApplicationInsightsMiddelware(TelemetryClient client, string applicationname)
        {
            _client = client;
            _applicationname = applicationname;
        }

        public void Execute(Context<HttpMessageWrapper> context, Action<Context<HttpMessageWrapper>> next)
        {
            var telemetry = new DependencyTelemetry()
            {
                Name = context.Data.Request.Uri.AbsolutePath,

                Id = context.Data.Request.Identity.Id,

                Timestamp = DateTime.Now,

                Target = context.Data.Request.Uri.Host,

                Data = context.Data.Request.Uri.ToString(),

                Type = "HTTP",
            };

            if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.OperationId))
            {
                telemetry.Context.Operation.Id = context.Data.Request.Identity.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.ParentId))
            {
                telemetry.Context.Operation.ParentId = context.Data.Request.Identity.ParentId;
            }

            if (!string.IsNullOrWhiteSpace(_applicationname))
            {
                telemetry.Context.Cloud.RoleName = _applicationname;
            }

            HttpResponse response = null;

            try
            {
                next(context);

                if (context.Data.Response.HttpStatusCode == HttpStatusCode.OK)
                {
                    telemetry.Success = true;

                    telemetry.ResultCode = "200";
                }
                else
                {
                    telemetry.Success = false;

                    if (context.Data.Response.HttpStatusCode != null)
                    {
                        telemetry.ResultCode = ((int)response.HttpStatusCode).ToString();
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

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.OperationId))
                {
                    telemetryexception.Context.Operation.Id = context.Data.Request.Identity.OperationId;
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.ParentId))
                {
                    telemetryexception.Context.Operation.ParentId = context.Data.Request.Identity.ParentId;
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

        public async Task ExecuteAsync(Context<HttpMessageWrapper> context, Func<Context<HttpMessageWrapper>, Task> next)
        {
            var telemetry = new DependencyTelemetry()
            {
                Name = context.Data.Request.Uri.AbsolutePath,

                Id = context.Data.Request.Identity.Id,

                Timestamp = DateTime.Now,

                Target = context.Data.Request.Uri.Host,

                Data = context.Data.Request.Uri.ToString(),

                Type = "HTTP",
            };

            if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.OperationId))
            {
                telemetry.Context.Operation.Id = context.Data.Request.Identity.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.ParentId))
            {
                telemetry.Context.Operation.ParentId = context.Data.Request.Identity.ParentId;
            }

            if (!string.IsNullOrWhiteSpace(_applicationname))
            {
                telemetry.Context.Cloud.RoleName = _applicationname;
            }

            HttpResponse response = null;

            try
            {

                await next(context);

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    telemetry.Success = true;

                    telemetry.ResultCode = "200";
                }
                else
                {
                    telemetry.Success = false;

                    if (response.HttpStatusCode != null)
                    {
                        telemetry.ResultCode = ((int)response.HttpStatusCode).ToString();
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

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.OperationId))
                {
                    telemetryexception.Context.Operation.Id = context.Data.Request.Identity.OperationId;
                }

                if (!string.IsNullOrWhiteSpace(context.Data.Request.Identity.ParentId))
                {
                    telemetryexception.Context.Operation.ParentId = context.Data.Request.Identity.ParentId;
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
