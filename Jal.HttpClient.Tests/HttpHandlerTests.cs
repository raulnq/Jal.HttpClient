using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Common.Logging.Installer;
using Jal.HttpClient.Model;
using NUnit.Framework;
using Shouldly;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Jal.ChainOfResponsability.Installer;
using Jal.Locator.CastleWindsor.Installer;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jal.HttpClient.Tests
{
    [TestClass]
    public class HttpHandlerTests
    {
        private IHttpHandler _sut;

        [TestInitialize]
        public void Setup()
        {
            var log = LogManager.GetLogger("logger");

            var container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<ILog>().Instance(log));

            container.Install(new HttpClientInstaller());

            container.Install(new HttpClientCommonLoggingInstaller());

            container.Install(new ChainOfResponsabilityInstaller());

            container.Install(new ServiceLocatorInstaller());

            _sut = container.Resolve<IHttpHandler>();
        }

        [TestMethod]
        public async Task Send_Get_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/get", HttpMethod.Get)))
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
        public async Task Send_GetWithHeaders_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Message.Headers.Add("Header1", "value");

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_GetWithDuplicatedHeaders_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Message.Headers.Add("Header1", "value");

            request.Message.Headers.Add("Header1", "value");

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_GetWithQueryParameters_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.QueryParameters.Add(new HttpQueryParameter("parameter", "value"));

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_GetXml_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/xml", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("xml");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/xml");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_GetHtml_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/html", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("<html>");

                response.Message.Content.Headers.ContentType.MediaType.ShouldContain("text/html");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        //[TestMethod]
        //public async Task Send_GetImagePng_Ok()
        //{
        //    using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/image/png", HttpMethod.Get)))
        //    {
        //        var image = Image.FromStream(await response.Message.Content.ReadAsStreamAsync());

        //        var same = ImageFormat.Png.Equals(image.RawFormat);

        //        same.ShouldBeTrue();

        //        response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

        //        response.Message.Content.Headers.ContentType.MediaType.ShouldBe("image/png");

        //        response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

        //        response.Exception.ShouldBeNull();
        //    }
        //}

        [TestMethod]
        public async Task Send_Get404_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/status/404", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("text/html");

                response.Message.StatusCode.ShouldBe(HttpStatusCode.NotFound);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_Get500_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/status/500", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("text/html");

                response.Message.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_PostStreamJsonUtf8_Ok()
        {
            var streamcontent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(@"{""message"":""Hello World!!""}")));

            streamcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = streamcontent
            };

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_PostStreamJsonUtf7_Ok()
        {
            var streamcontent = new StreamContent(new MemoryStream(Encoding.UTF7.GetBytes(@"{""message"":""Hello World!!""}")));

            streamcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = "utf-7"
            };

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = streamcontent
            };

            using (var response = await _sut.SendAsync(request))
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
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"<message>Hello World!!</message>", Encoding.UTF8, "text/xml")
            };

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_PostFormUrlEncodedUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("message", "hello world") } )
            };

            request.Message.Content.Headers.ContentType.CharSet = "utf-7";

            using (var response = await _sut.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("hello world");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostFormUrlEncodedUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("message", "hello world") })
            };

            using (var response = await _sut.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("hello world");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PostJsonUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF7, "application/json")
            };

            using (var response = await _sut.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [TestMethod]
        public async Task Send_PutJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/put", HttpMethod.Put)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_Delete_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/delete", HttpMethod.Delete)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = await _sut.SendAsync(request))
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
        public async Task Send_Get_TimeOut()
        {
            var client = new System.Net.Http.HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(10)
            };
            var request = new HttpRequest("http://httpbin.org/delay/5", HttpMethod.Get, client)
                          {
                          };

            using (var response = await _sut.SendAsync(request))
            {
                response.Message.ShouldBeNull();

                response.Exception.ShouldNotBeNull();
            }
        }

        [TestMethod]
        public async Task Send_GetGzip_Ok()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpclient = new System.Net.Http.HttpClient(handler);

            var request = new HttpRequest("http://httpbin.org/gzip", HttpMethod.Get, httpclient);

            using (var response = await _sut.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("gzipped");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                //response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        //[Test]
        //public void Send_GetDeflate_Ok()
        //{
        //    var request = new HttpRequest("http://httpbin.org/deflate", HttpMethod.Get)
        //    {
        //        Decompression = DecompressionMethods.Deflate
        //    };

        //    using (var response = await _sut.SendAsync(request))
        //    {
        //        var content = response.Message.Content.Read();

        //        response.Message.Content.IsString().ShouldBeTrue();

        //        content.ShouldContain("gzipped");

        //        response.Message.Content.ContentType.ShouldBe("application/json");

        //        response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

        //        response.HttpExceptionStatus.ShouldBeNull();

        //        response.Exception.ShouldBeNull();
        //    }
        //}
    }
}
