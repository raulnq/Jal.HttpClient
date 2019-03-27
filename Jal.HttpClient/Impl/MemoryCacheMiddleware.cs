﻿using System;
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
    public class MemoryCacheMiddleware : IMiddlewareAsync<HttpWrapper>
    {
        public async Task ExecuteAsync(Context<HttpWrapper> context, Func<Context<HttpWrapper>, Task> next)
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
                            if (context.Data.Request.Context["oncacheget"] is Action<HttpResponseMessage> oncacheget)
                            {
                                oncacheget(resultfromcache);
                            }
                        }

                        var copyfromcache = await resultfromcache.Clone();

                        context.Data.Response = new HttpResponse(context.Data.Request)
                        {
                            Message = copyfromcache
                        };

                        return;
                    }

                    await next(context);

                    var copy = await context.Data.Response.Message.Clone();

                    var copyforcache = await copy.Clone();

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