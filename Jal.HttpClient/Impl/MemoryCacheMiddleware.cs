using System;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using System.Runtime.Caching;
using Jal.HttpClient.Model;
using System.IO;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using System.Net.Http;
using Jal.HttpClient.Extensions;

namespace Jal.HttpClient.Impl
{
    public class MemoryCacheMiddleware : IMiddleware<HttpMessageWrapper>, IMiddlewareAsync<HttpMessageWrapper>
    {

        public void Execute(Context<HttpMessageWrapper> context, Action<Context<HttpMessageWrapper>> next)
        {
            if (context.Data.Request.Context.ContainsKey("durationinseconds") && context.Data.Request.Context.ContainsKey("cachemode") && context.Data.Request.Context.ContainsKey("keybuilder"))
            {
                var durationinseconds = context.Data.Request.Context["durationinseconds"] as double?;

                var mode = context.Data.Request.Context["cachemode"] as string;

                var keybuilder = context.Data.Request.Context["keybuilder"] as Func<HttpRequest, string>;

                if (durationinseconds != null && !string.IsNullOrWhiteSpace(mode) && keybuilder != null)
                {
                    var cache = MemoryCache.Default;

                    var key = keybuilder(context.Data.Request);

                    if (cache.Contains(key))
                    {
                        var resultfromcache = cache[context.Data.Request.Message.RequestUri.AbsoluteUri] as HttpResponseMessage;

                        if (context.Data.Request.Context.ContainsKey("oncacheget"))
                        {
                            var oncacheget = context.Data.Request.Context["oncacheget"] as Action<HttpResponseMessage>;

                            if (oncacheget != null)
                            {
                                oncacheget(resultfromcache);
                            }
                        }

                        var copyfromcache = resultfromcache.Clone().GetAwaiter().GetResult();

                        context.Data.Response = new HttpResponse(context.Data.Request)
                        {
                            Message = copyfromcache
                        };

                        return;
                    }

                    next(context);

                    var copy = context.Data.Response.Message.Clone().GetAwaiter().GetResult();

                    var copyforcache = copy.Clone().GetAwaiter().GetResult();

                    context.Data.Response.Message = copy;

                    var policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(durationinseconds.Value) };

                    if (mode == "absolute")
                    {
                        policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(durationinseconds.Value) };
                    }

                    cache.Set(key, copyforcache, policy);
                }
                else
                {
                    next(context);
                }
            }
            else
            {
                next(context);
            }
        }

        public async Task ExecuteAsync(Context<HttpMessageWrapper> context, Func<Context<HttpMessageWrapper>, Task> next)
        {
            if (context.Data.Request.Context.ContainsKey("durationinseconds") && context.Data.Request.Context.ContainsKey("cachemode") && context.Data.Request.Context.ContainsKey("keybuilder"))
            {
                var durationinseconds = context.Data.Request.Context["durationinseconds"] as double?;

                var mode = context.Data.Request.Context["cachemode"] as string;

                var keybuilder = context.Data.Request.Context["keybuilder"] as Func<HttpRequest, string>;

                if (durationinseconds != null && !string.IsNullOrWhiteSpace(mode) && keybuilder != null)
                {
                    var cache = MemoryCache.Default;

                    var key = keybuilder(context.Data.Request);

                    if (cache.Contains(key))
                    {
                        var resultfromcache = cache[context.Data.Request.Message.RequestUri.AbsoluteUri] as HttpResponseMessage;

                        if (context.Data.Request.Context.ContainsKey("oncacheget"))
                        {
                            var oncacheget = context.Data.Request.Context["oncacheget"] as Action<HttpResponseMessage>;

                            if (oncacheget != null)
                            {
                                oncacheget(resultfromcache);
                            }
                        }

                        var copyfromcache = await resultfromcache.Clone();

                        context.Data.Response.Message = copyfromcache;

                        return;
                    }

                    await next(context);

                    var copy = await context.Data.Response.Message.Clone();

                    context.Data.Response.Message = copy;

                    var policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(durationinseconds.Value) };

                    if (mode == "absolute")
                    {
                        policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(durationinseconds.Value) };
                    }

                    cache.Set(key, copy, policy);
                }
                else
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }
    }
}
