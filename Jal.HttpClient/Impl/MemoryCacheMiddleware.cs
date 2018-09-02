using System;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using System.Runtime.Caching;
using Jal.HttpClient.Model;
using System.IO;

namespace Jal.HttpClient.Impl
{
    public class MemoryCacheMiddleware : IHttpMiddleware
    {
        public HttpResponse Send(HttpRequest request, Func<HttpRequest, HttpContext, HttpResponse> next, HttpContext context)
        {
            if (request.Context.ContainsKey("durationinseconds") && request.Context.ContainsKey("cachemode") && request.Context.ContainsKey("keybuilder"))
            {
                var durationinseconds = request.Context["durationinseconds"] as double?;

                var mode = request.Context["cachemode"] as string;

                var keybuilder = request.Context["keybuilder"] as Func<HttpRequest, string>;

                if (durationinseconds != null && !string.IsNullOrWhiteSpace(mode) && keybuilder != null)
                {
                    var cache = MemoryCache.Default;

                    var key = keybuilder(request);

                    if (cache.Contains(key))
                    {
                        var resultfromcache = cache[request.Url] as HttpResponse;

                        if (request.Context.ContainsKey("oncacheget"))
                        {
                            var oncacheget = request.Context["oncacheget"] as Action<HttpResponse>;

                            if (oncacheget != null)
                            {
                                oncacheget(resultfromcache);
                            }
                        }

                        var copyfromcache = Copy(resultfromcache);

                        return copyfromcache;
                    }

                    var result = next(request, context);

                    var copy = Copy(result);

                    var policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(durationinseconds.Value) };

                    if (mode == "absolute")
                    {
                        policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(durationinseconds.Value) };
                    }

                    cache.Set(key, result, policy);

                    return copy;
                }
            }

            return next(request, context);
        }

        private static HttpResponse Copy(HttpResponse response)
        {
            var copy = new HttpResponse()
            {
                Duration = response.Duration,
                Exception = response.Exception,
                HttpExceptionStatus = response.HttpExceptionStatus,
                Uri = response.Uri,
                HttpStatusCode = response.HttpStatusCode,
                Headers = response.Headers,
                Content = new HttpResponseContent()
                {
                    CharacterSet = response.Content.CharacterSet,
                    ContentLength = response.Content.ContentLength,
                    ContentType = response.Content.ContentType
                }
            };

            copy.Content.Stream = new MemoryStream();
            response.Content.Stream.Position = 0;
            response.Content.Stream.CopyTo(copy.Content.Stream);
            return copy;
        }

        public async Task<HttpResponse> SendAsync(HttpRequest request, Func<HttpRequest, HttpContext, Task<HttpResponse>> next, HttpContext context)
        {
            if (request.Context.ContainsKey("durationinseconds") && request.Context.ContainsKey("mode") && request.Context.ContainsKey("keybuilder"))
            {
                var durationinseconds = request.Context["durationinseconds"] as double?;

                var mode = request.Context["cachemode"] as string;

                var keybuilder = request.Context["keybuilder"] as Func<HttpRequest, string>;

                if (durationinseconds != null && !string.IsNullOrWhiteSpace(mode) && keybuilder!=null)
                {
                    var cache = MemoryCache.Default;

                    var key = keybuilder(request);

                    if (cache.Contains(key))
                    {
                        var resultfromcache = cache[request.Url] as HttpResponse;

                        if (request.Context.ContainsKey("oncacheget"))
                        {
                            var oncacheget = request.Context["oncacheget"] as Action<HttpResponse>;

                            if (oncacheget != null)
                            {
                                oncacheget(resultfromcache);
                            }
                        }

                        var copyfromcache = Copy(resultfromcache);

                        return copyfromcache;
                    }

                    var result = await next(request, context);

                    var copy = Copy(result);

                    var policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(durationinseconds.Value) };

                    if (mode == "absolute")
                    {
                        policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(durationinseconds.Value) };
                    }

                    cache.Set(key, result, policy);

                    return copy;
                }
            }

            return await next(request, context);
        }
    }
}
