using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Common.Logging;
using Jal.HttpClient.Common.Logging.Installer;
using NUnit.Framework;
using Shouldly;
using Jal.HttpClient.Polly.Installer;
using Jal.HttpClient.Polly;
using Jal.HttpClient.Extensions;
using Jal.Locator.CastleWindsor.Installer;
using Jal.ChainOfResponsability.Installer;
using System;
using Polly.CircuitBreaker;
using Polly;
using Jal.HttpClient.Model;
using Jal.HttpClient.Serilog.Installer;
using Jal.HttpClient.Serilog;
using Serilog;

namespace Jal.HttpClient.Tests
{
    [TestFixture]
    public class HttpHandlerBuilderTests
    {
        private IHttpFluentHandler _sut;

        [SetUp]
        public void Setup()
        {
            var log = LogManager.GetLogger("logger");

            var container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

            container.Register(Component.For<ILog>().Instance(log));

            container.Install(new HttpClientInstaller());

            container.Install(new ChainOfResponsabilityInstaller());

            container.Install(new ServiceLocatorInstaller());

            container.Install(new HttpClientCommonLoggingInstaller());

            container.Install(new HttpClientSerilogInstaller());

            container.Install(new HttpClientPollyInstaller());

            _sut = container.Resolve<IHttpFluentHandler>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}")
                .CreateLogger();
        }

        [Test]
        public async Task Send_Get_Ok()
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

        [Test]
        public async Task Send_Get_Authorized_Ok()
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

        [Test]
        public async Task Send_Get_Retry_Ok()
        {
            var retries = 0;
            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => {
                x.AuthorizedByToken("token", "value");
                x.OnConditionRetry(3, y => y.Message.StatusCode == HttpStatusCode.OK, (z, c) => { retries++; });
                x.UseCommonLogging();
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

        [Test]
        public async Task Send_Get_AuthorizationAndRetry_Ok()
        {
            var retries = 0;
            using (var response = await _sut.Get("http://httpbin.org/get")
                .WithMiddleware(
                    x => x.OnConditionRetry(3, y => y.Message.StatusCode == HttpStatusCode.OK, (z, c) => { retries++; }))
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

        [Test]
        public async Task Send_Get_CircuitBreaker_Ok()
        {
            CircuitBreakerPolicy<HttpResponse> breakerPolicy = Policy
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

        [Test]
        public async Task Send_Get_MemoryCache_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => { x.AddTrackingInformation(); x.UseCommonLogging(); x.UseMemoryCache(30, y => y.Message.RequestUri.AbsoluteUri, z => z.Message.StatusCode == HttpStatusCode.OK); }).WithIdentity("a","b","c").WithHeaders(x => x.Add("header", "old")).SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("header");

                content.ShouldContain("old");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }

            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => { x.AddTrackingInformation(); x.UseCommonLogging(); x.UseMemoryCache(30, y => y.Message.RequestUri.AbsoluteUri, z => z.Message.StatusCode == HttpStatusCode.OK); }).WithHeaders(x => x.Add("header", "new")).SendAsync())
            {

                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("header");

                content.ShouldContain("old");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
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

        [Test]
        public async Task Send_PostJsonUtf8_Ok()
        {
            using (var response = await _sut.Post("http://httpbin.org/post").WithMiddleware(x => x.UseSerilog()).Json(@"{""message"":""Hello World!!""}").SendAsync())
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public async Task Send_GetWithQueryParameters_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/get").WithMiddleware(x => x.UseSerilog()).WithQueryParameters(x=>x.Add("parameter","value")).SendAsync())
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

        [Test]
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

        [Test]
        public async Task Send_Delete_Ok()
        {
            using (var response = await _sut.Delete("http://httpbin.org/delete").SendAsync())
            {

            }
        }

        [Test]
        public async Task Send_Get_TimeOut()
        {
            var client = new System.Net.Http.HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(10)
            };
            using (var response = await _sut.Delete("https://httpbin.org/delay/5", client).SendAsync())
            {

            }
        }

        //[Test]
        //public void Send_GetGzip_Ok()
        //{
        //    var container = new WindsorContainer();

        //    container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

        //    container.Install(new HttpClientInstaller());

        //    var httpclientbuilder = container.Resolve<IHttpFluentHandler>();

        //    var response = httpclientbuilder.Get("http://httpbin.org/gzip").GZip().Send;
        //}
    }
}
