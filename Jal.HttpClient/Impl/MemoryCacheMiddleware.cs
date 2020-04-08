using System;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Net.Http;
using Jal.ChainOfResponsability;

namespace Jal.HttpClient
{

    public class MemoryCacheMiddleware : IAsyncMiddleware<HttpContext>
    {
        public const string CACHE_DURATION_IN_SECONDS_KEY = "cachedurationinseconds";

        public const string CACHE_MODE_KEY = "cachemode";

        public const string CACHE_WHEN_KEY = "cachewhen";

        public const string CACHE_KEY_BUILDER_KEY = "cachekeybuilder";

        public const string ON_CACHE_GET = "oncacheget";

        public async Task ExecuteAsync(AsyncContext<HttpContext> context, Func<AsyncContext<HttpContext>, Task> next)
        {
            if (context.Data.Request.Context.ContainsKey(CACHE_DURATION_IN_SECONDS_KEY) && context.Data.Request.Context.ContainsKey(CACHE_MODE_KEY) && 
                context.Data.Request.Context.ContainsKey(CACHE_KEY_BUILDER_KEY) && context.Data.Request.Context.ContainsKey(CACHE_WHEN_KEY))
            {
                var durationinseconds = context.Data.Request.Context[CACHE_DURATION_IN_SECONDS_KEY] as double?;

                var mode = context.Data.Request.Context[CACHE_MODE_KEY] as string;

                var when = context.Data.Request.Context[CACHE_WHEN_KEY] as Func<HttpResponse, bool>;

                var keybuilder = context.Data.Request.Context[CACHE_KEY_BUILDER_KEY] as Func<HttpRequest, string>;

                if (durationinseconds != null && !string.IsNullOrWhiteSpace(mode) && keybuilder != null)
                {
                    var cache = MemoryCache.Default;

                    var key = keybuilder(context.Data.Request);

                    if (cache.Contains(key))
                    {
                        var resultfromcache = cache[key] as HttpResponseMessage;

                        if (context.Data.Request.Context.ContainsKey(ON_CACHE_GET))
                        {
                            if (context.Data.Request.Context[ON_CACHE_GET] is Action<HttpResponseMessage> oncacheget)
                            {
                                oncacheget(resultfromcache);
                            }
                        }

                        var copyfromcache = await resultfromcache.Clone();

                        context.Data.Response = new HttpResponse(context.Data.Request, copyfromcache, null, 0);

                        return;
                    }

                    await next(context);

                    if (context.Data.Response.Message!=null && when(context.Data.Response))
                    {
                        var copy = await context.Data.Response.Message.Clone();

                        var copyforcache = await copy.Clone();

                        var policy = new CacheItemPolicy { SlidingExpiration = TimeSpan.FromSeconds(durationinseconds.Value) };

                        if (mode == "absolute")
                        {
                            policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(durationinseconds.Value) };
                        }

                        copyforcache.Headers.Add("from-cache", "yes");

                        cache.Set(key, copyforcache, policy);

                        context.Data.Response = new HttpResponse(context.Data.Request, copy, context.Data.Response.Exception, context.Data.Response.Duration);
                    }
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
