using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Jal.HttpClient.Installer;
using Shouldly;
using System;
using Serilog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Jal.HttpClient.Common.Logging;
using Jal.HttpClient.Serilog;
using Jal.HttpClient.Polly;
using Polly;
using Polly.CircuitBreaker;

namespace Jal.HttpClient.Tests
{
    [TestClass]
    public class HttpHandlerBuilderTests
    {
        private IHttpFluentHandler _sut;

        [TestInitialize]
        public void Setup()
        {
            var log = LogManager.GetLogger("logger");

            var container = new WindsorContainer();

            container.Register(Component.For<ILog>().Instance(log));

            container.AddHttpClient(c=>
            {
                c.Add<CommonLoggingMiddelware>();

                c.Add<SerilogMiddelware>();

                c.Add<CircuitBreakerMiddelware>();

                c.Add<TimeoutMiddelware>();

                c.Add<OnConditionRetryMiddelware>();
            });

            _sut = container.GetHttpClient();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
                .CreateLogger();
        }

        [TestMethod]
        public async Task Send_Get_Serilog_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/ip").WithMiddleware(x=>x.UseSerilog()).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("origin");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get_AuthorizedByToken_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => x.AuthorizedByToken("token","value")).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("token");

                content.ShouldContain("value");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get_Middlewares_Ok()
        {
            var retries = 0;
            using (var response = await _sut.Get("http://httpbin.org/get?hi=1").WithMiddleware(x => {
                x.AddTracing();
                x.AuthorizedByToken("token", "value");
                x.OnConditionRetry(3, y => y.Message.StatusCode == HttpStatusCode.OK, (z, c) => { retries++; });
                x.UseSerilog();
                x.UseMemoryCache(30, y => y.Message.RequestUri.AbsoluteUri, z => z.Message.StatusCode == HttpStatusCode.OK);
            }).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                retries.ShouldBe(3);

                content.ShouldContain("token");

                content.ShouldContain("value");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get_Retry_Ok()
        {
            var retries = 0;
            using (var response = await _sut.Get("http://httpbin.org/get")
                .WithMiddleware(x => x.OnConditionRetry(3, y => y.Message.StatusCode == HttpStatusCode.OK, (z, c) => { retries++; }))
                .SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                retries.ShouldBe(3);

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get_CircuitBreaker_Ok()
        {
            AsyncCircuitBreakerPolicy<HttpResponse> breakerPolicy = Policy
            .HandleResult<HttpResponse>(r => r.Message?.StatusCode!= HttpStatusCode.OK )
            .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10));

            using (var response = await _sut.Get("http://httpbin.org/status/500").WithMiddleware(x => x.UseCircuitBreaker(breakerPolicy)).SendAsync())
            {
                response.Message.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            }

            using (var response = await _sut.Get("http://httpbin.org/status/500").WithMiddleware(x => x.UseCircuitBreaker(breakerPolicy)).SendAsync())
            {
                response.Message.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            }

            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => x.UseCircuitBreaker(breakerPolicy)).SendAsync())
            {
                response.Message.ShouldBeNull();
            }

            await Task.Delay(TimeSpan.FromSeconds(15));

            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => x.UseCircuitBreaker(breakerPolicy)).SendAsync())
            {

                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get_MemoryCache_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => { x.UseMemoryCache(30, y => y.Message.RequestUri.AbsoluteUri, z => z.Message.StatusCode == HttpStatusCode.OK); }).WithHeaders(x => x.Add("header", "old")).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("header");

                content.ShouldContain("old");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }

            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => { x.UseCommonLogging(); x.UseMemoryCache(30, y => y.Message.RequestUri.AbsoluteUri, z => z.Message.StatusCode == HttpStatusCode.OK); }).WithHeaders(x => x.Add("header", "new")).SendAsync())
            {

                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("header");

                content.ShouldContain("old");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Message.Headers.ShouldContain(x => x.Key == "from-cache");

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task SendAsync_Get_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/ip").SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("origin");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostJsonUtf8_Ok()
        {
            using (var response = await _sut.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostXmlUtf8_Ok()
        {
            using (var response = await _sut.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostFormUrlEncodedArrayUtf8_Ok()
        {
            using (var response = await _sut.Post("http://httpbin.org/post").FormUrlEncoded(new[] { new KeyValuePair<string, string>("message", "Hello World"), new KeyValuePair<string, string>("array", "a a"), new KeyValuePair<string, string>("array", "bbb"), new KeyValuePair<string, string>("array", "c c"), }).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostFormUrlEncodedUtf8_Ok()
        {
            using (var response = await _sut.Post("http://httpbin.org/post").FormUrlEncoded(new [] {new KeyValuePair<string, string>("message", "Hello World") }).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostMultiPartFormDataUtf8_Ok()
        {
            using (var response = await _sut.Post("http://httpbin.org/post").MultiPartFormData(x =>
             {
                 x.Json(@"{""message1"":""Hello World1!!""}", "form-data");
                 x.Json(@"{""message2"":""Hello World2!!""}", "form-data");
                 x.Xml("<saludo>hola mundo</saludo>", "nombre3", "file.xml");
                 x.WithContent("message3").WithDisposition("form-data");
                 x.UrlEncoded("a", "message4");
                 x.UrlEncoded("b", "message4");
                 x.UrlEncoded("c c", "message4");
                 //x.WithContent(new FileStream("file.zip", FileMode.Open, FileAccess.Read)).WithDisposition("file", "file.zip");
             }).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();
            }
        }

        [TestMethod]
        public async Task Send_GetWithQueryParameters_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/get").WithQueryParameters(x=>x.Add("parameter","value")).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("parameter");

                content.ShouldContain("value");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_GetWithHeaders_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/get").WithHeaders(x => x.Add("Header1", "value")).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Header1");

                content.ShouldContain("value");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Delete_Ok()
        {
            using (var response = await _sut.Delete("http://httpbin.org/delete").SendAsync())
            {
                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task Send_Get_TimeOut()
        {
            using (var response = await _sut.Get("https://httpbin.org/delay/5").WithMiddleware(x=>x.UseTimeout(2)).SendAsync())
            {
                response.Message?.Content.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get_CancellationToken()
        {
            using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(3)))
            {
                using (var response = await _sut.Get("https://httpbin.org/delay/5", cancellationtoken: source.Token).SendAsync())
                {
                    response.Message?.Content.ShouldBeNull();
                }
            }
        }
    }
}
