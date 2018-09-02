using System;
using System.Net;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.HttpClient.ApplicationInsights
{
    public class ApplicationInsightsMiddelware : IHttpMiddleware
    {
        private readonly TelemetryClient _client;

        private readonly string _applicationname;

        public ApplicationInsightsMiddelware(TelemetryClient client, string applicationname)
        {
            _client = client;
            _applicationname = applicationname;
        }

        public HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context)
        {
            var telemetry = new DependencyTelemetry()
            {
                Name = request.Uri.AbsolutePath,

                Id = request.Identity.Id,

                Timestamp = DateTime.Now,

                Target = request.Uri.Host,

                Data = request.Uri.ToString(),

                Type = "HTTP",
            };

            if(!string.IsNullOrWhiteSpace(request.Identity.OperationId))
            {
                telemetry.Context.Operation.Id = request.Identity.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(request.Identity.ParentId))
            {
                telemetry.Context.Operation.ParentId = request.Identity.ParentId;
            }

            if (!string.IsNullOrWhiteSpace(_applicationname))
            {
                telemetry.Context.Cloud.RoleName = _applicationname;
            }

            HttpResponse response = null;

            try
            {
                response = next(request, context);

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    telemetry.Success = true;

                    telemetry.ResultCode = "200";
                }
                else
                {
                    telemetry.Success = false;

                    if(response.HttpStatusCode!=null)
                    {
                        telemetry.ResultCode = ((int)response.HttpStatusCode).ToString();
                    }
                    else
                    {
                        telemetry.ResultCode = "500";
                    }
                }

                return response;
            }
            catch (Exception exception)
            {
                telemetry.Success = false;

                telemetry.ResultCode = "500";

                var telemetryexception = new ExceptionTelemetry(exception);

                if (!string.IsNullOrWhiteSpace(request.Identity.OperationId))
                {
                    telemetryexception.Context.Operation.Id = request.Identity.OperationId;
                }

                if (!string.IsNullOrWhiteSpace(request.Identity.ParentId))
                {
                    telemetryexception.Context.Operation.ParentId = request.Identity.ParentId;
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
                if(response!=null)
                {
                    telemetry.Duration = TimeSpan.FromMilliseconds(response.Duration);
                }

                _client.TrackDependency(telemetry);
            }
        }


        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context)
        {

            var telemetry = new DependencyTelemetry()
            {
                Name = request.Uri.AbsolutePath,

                Id = request.Identity.Id,

                Timestamp = DateTime.Now,

                Target = request.Uri.Host,

                Data = request.Uri.ToString(),

                Type = "HTTP",
            };

            if (!string.IsNullOrWhiteSpace(request.Identity.OperationId))
            {
                telemetry.Context.Operation.Id = request.Identity.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(request.Identity.ParentId))
            {
                telemetry.Context.Operation.ParentId = request.Identity.ParentId;
            }

            if (!string.IsNullOrWhiteSpace(_applicationname))
            {
                telemetry.Context.Cloud.RoleName = _applicationname;
            }

            HttpResponse response = null;

            try
            {

                response = await next(request, context);

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

                return response;
            }
            catch (Exception exception)
            {
                telemetry.Success = false;

                telemetry.ResultCode = "500";

                var telemetryexception = new ExceptionTelemetry(exception);

                if (!string.IsNullOrWhiteSpace(request.Identity.OperationId))
                {
                    telemetryexception.Context.Operation.Id = request.Identity.OperationId;
                }

                if (!string.IsNullOrWhiteSpace(request.Identity.ParentId))
                {
                    telemetryexception.Context.Operation.ParentId = request.Identity.ParentId;
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
