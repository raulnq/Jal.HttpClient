using System.Net;
using Castle.Windsor;
using Jal.HttpClient.Fluent;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface;
using NUnit.Framework;
using Shouldly;

namespace Jal.HttpClient.Tests
{
    [TestFixture]
    public class HttpHandlerBuilderTests
    {
        [Test]
        public void Send_Get_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/ip").Send();

            response.Content.ShouldContain("origin");

            response.Bytes.ShouldNotBeNull();

            response.WebException.ShouldBeNull();

            response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public void SendAsync_Get_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var task = httpclientbuilder.Get("http://httpbin.org//get").SendAsync();

            var response = task.Result;

            response.Content.ShouldContain("origin");

            response.Bytes.ShouldNotBeNull();

            response.WebException.ShouldBeNull();

            response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public void Send_GetXml_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/xml").Send();

            response.Content.ShouldContain("xml");

            response.Bytes.ShouldNotBeNull();

            response.WebException.ShouldBeNull();

            response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public void Send_GetHtml_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/html").Send();
        }

        [Test]
        public void Send_GetImagePng_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/image/png").Send();
        }

        [Test]
        public void Send_PostJsonUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf8().Send();
        }

        [Test]
        public void Send_PostXmlUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Post("http://httpbin.org/post").Xml(@"<message>Hello World!!</message>").Utf8().Send();
        }

        [Test]
        public void Send_PostFormUrlEncodedUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Post("http://httpbin.org/post").FormUrlEncoded(@"message=Hello%World!!").Utf8().Send();
        }

        [Test]
        public void Send_PostJsonUtf16_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf16().Send();
        }

        [Test]
        public void Send_PutJsonUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Post("http://httpbin.org/post").Json(@"{""message"":""Hello World!!""}").Utf8().Send();
        }

        [Test]
        public void Send_Delete_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Delete("http://httpbin.org/delete").Send();
        }

        [Test]
        public void Send_Get_TimeOut()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/delay/5").WithTimeout(10).Send();
        }

        [Test]
        public void Send_GetGzip_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/gzip").GZip().Send();
        }

        [Test]
        public void Send_GetDeflate_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclientbuilder = container.Resolve<IHttpHandlerBuilder>();

            var response = httpclientbuilder.Get("http://httpbin.org/deflate").Send();
        }
    }
}
