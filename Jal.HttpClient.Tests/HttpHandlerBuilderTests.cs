using System.IO;
using System.Net;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Jal.HttpClient.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Logger.Installer;
using NUnit.Framework;
using Shouldly;

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

            container.Register(Component.For<ILog>().Instance(log));

            container.Install(new HttpClientInstaller());

            container.Install(new HttpClienLoggertInstaller());

            _sut = container.Resolve<IHttpFluentHandler>();
        }

        [Test]
        public void Send_Get_Ok()
        {
            using (var response = _sut.Get("http://httpbin.org/ip").Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("origin");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public async void SendAsync_Get_Ok()
        {
            using (var response = await _sut.Get("http://httpbin.org/ip").SendAsync)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("origin");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostJsonUtf8_Ok()
        {
            using (var response = _sut.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("Hello World");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostXmlUtf8_Ok()
        {
            using (var response = _sut.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("Hello World");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostFormUrlEncodedUtf8_Ok()
        {
            using (var response = _sut.Post("http://httpbin.org/post").FormUrlEncoded(@"message=Hello%20World!!").Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("Hello%20World");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostMultiPartFormDataUtf8_Ok()
        {
            using (var response = _sut.Post("http://httpbin.org/post").WithTimeout(60000).WithAllowWriteStreamBuffering(false).MultiPartFormData(x =>
             {
                 x.Json(@"{""message1"":""Hello World1!!""}", "nombre1");
                 x.Json(@"{""message2"":""Hello World2!!""}", "nombre2");
                 x.Xml("<saludo>hola mundo</saludo>", "nombre3", "file.xml");
                 //x.WithContent(new FileStream("file.zip", FileMode.Open, FileAccess.Read)).WithDisposition("file", "file.zip");
             }).Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();
            }
        }

        [Test]
        public void Send_GetWithQueryParameters_Ok()
        {
            using (var response = _sut.Get("http://httpbin.org/get").WithQueryParameters(x=>x.Add("parameter","value")).Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("parameter");

                content.ShouldContain("value");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetWithHeaders_Ok()
        {
            using (var response = _sut.Get("http://httpbin.org/get").WithHeaders(x => x.Add("Header1", "value")).Send)
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("Header1");

                content.ShouldContain("value");

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_Delete_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpFluentHandler>();

            var response = httpclientbuilder.Delete("http://httpbin.org/delete").Send;
        }

        [Test]
        public void Send_Get_TimeOut()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpFluentHandler>();

            var response = httpclientbuilder.Get("http://httpbin.org/delay/5").WithTimeout(10).Send;
        }

        [Test]
        public void Send_GetGzip_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpFluentHandler>();

            var response = httpclientbuilder.Get("http://httpbin.org/gzip").GZip().Send;
        }
    }
}
