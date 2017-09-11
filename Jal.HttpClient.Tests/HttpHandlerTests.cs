using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Castle.Windsor;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using NUnit.Framework;

namespace Jal.HttpClient.Tests
{
    [TestFixture]
    public class HttpHandlerTests
    {
        [Test]
        public void Send_Get_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("http://httpbin.org/ip", HttpMethod.Get));
        }

        [Test]
        public void SendAsync_Get_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var task = httpclient.SendAsync(new HttpRequest("http://httpbin.org/get", HttpMethod.Get));

            var response = task.Result;
        }

        [Test]
        public void Send_GetXml_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("http://httpbin.org/xml", HttpMethod.Get));
        }

        [Test]
        public void Send_GetHtml_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("http://httpbin.org/html", HttpMethod.Get));
        }

        [Test]
        public void Send_GetImagePng_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("http://httpbin.org/image/png", HttpMethod.Get));
        }

        [Test]
        public void Send_Get404_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("http://httpbin.org/status/404", HttpMethod.Get));
        }

        [Test]
        public void Send_Get500_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("http://httpbin.org/status/500", HttpMethod.Get));
        }

        [Test]
        public void Send_PostJsonUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
                          {
                            Content = new HttpStringContent(@"{""message"":""Hello World!!""}")
                            {
                                ContentType = "application/json",
                                CharacterSet = "charset=UTF-8"
                            }
                          };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_PostStreamJsonUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpStreamContent(new MemoryStream(Encoding.UTF8.GetBytes(@"{""message"":""Hello World!!""}")))
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=UTF-8"
                }
            };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_PostXmlUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpStringContent("<message>Hello World!!</message>")
                {
                    ContentType = "text/xml",
                    CharacterSet = "charset=UTF-8"
                }
            };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_PostFormUrlEncodedUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content =  new HttpStringContent("message=Hello%World!!")
                {
                    ContentType = "application/x-www-form-urlencoded",
                    CharacterSet = "charset=UTF-8"
                }
            };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_PostJsonUtf16_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpStringContent(@"{""message"":""Hello World!!""}")
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=UTF-16"
                }
            };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_PutJsonUtf8_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/put", HttpMethod.Put)
            {
                Content =  new HttpStringContent(@"{""message"":""Hello World!!""}")
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=UTF-8"
                }
            };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_Delete_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/delete", HttpMethod.Delete);

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_Get_TimeOut()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/delay/5", HttpMethod.Get)
                          {
                              Timeout = 10
                          };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_GetGzip_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/gzip", HttpMethod.Get)
                          {
                              DecompressionMethods = DecompressionMethods.GZip
                          };

            var response = httpclient.Send(request);
        }

        [Test]
        public void Send_GetDeflate_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var request = new HttpRequest("http://httpbin.org/deflate", HttpMethod.Get);

            var response = httpclient.Send(request);
        }
    }
}
