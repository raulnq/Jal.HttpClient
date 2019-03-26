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

namespace Jal.HttpClient.Tests
{
    [TestFixture]
    public class HttpHandlerTests
    {
        private IHttpHandler _sut;

        [SetUp]
        public void Setup()
        {
            var log = LogManager.GetLogger("logger");

            var container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

            container.Register(Component.For<ILog>().Instance(log));

            container.Install(new HttpClientInstaller());

            container.Install(new HttpClientCommonLoggingInstaller());

           _sut = container.Resolve<IHttpHandler>();
        }

        [Test]
        public void Send_Get_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/ip", HttpMethod.Get)))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("origin");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();    
            }
        }

        [Test]
        public async Task SendAsync_Get_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/get", HttpMethod.Get)))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("origin");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetWithHeaders_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Headers.Add("Header1", "value");

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Header1");

                content.ShouldContain("value");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetWithDuplicatedHeaders_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Headers.Add("Header1", "value");

            request.Headers.Add("Header1", "value");

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Header1");

                content.ShouldContain("value");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetWithQueryParameters_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.QueryParameters.Add(new HttpQueryParameter("parameter", "value"));

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("parameter");

                content.ShouldContain("value");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }
        [Test]
        public void Send_GetXml_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/xml", HttpMethod.Get)))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("xml");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/xml");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetHtml_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/html", HttpMethod.Get)))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("<html>");

                response.Content.Headers.ContentType.MediaType.ShouldContain("text/html");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetImagePng_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/image/png", HttpMethod.Get)))
            {
                var image = Image.FromStream(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());

                var same = ImageFormat.Png.Equals(image.RawFormat);

                same.ShouldBeTrue();

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Content.Headers.ContentType.MediaType.ShouldBe("image/png");

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_Get404_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/status/404", HttpMethod.Get)))
            {
                response.Content.Headers.ContentType.MediaType.ShouldBe("text/html");

                response.HttpStatusCode.ShouldBe(HttpStatusCode.NotFound);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_Get500_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/status/500", HttpMethod.Get)))
            {
                response.Content.Headers.ContentType.MediaType.ShouldBe("text/html");

                response.HttpStatusCode.ShouldBe(HttpStatusCode.InternalServerError);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostStreamJsonUtf8_Ok()
        {
            var c = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(@"{""message"":""Hello World!!""}")));

            c.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            c.Headers.ContentEncoding.Add("utf-8");

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                //Content = new HttpRequestStreamContent(new MemoryStream(Encoding.UTF8.GetBytes(@"{""message"":""Hello World!!""}")))
                //{
                //    ContentType = "application/json",
                //    CharacterSet = "charset=utf-8"
                //},
                Content = c,
                Timeout = 60000
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostStreamJsonUtf7_Ok()
        {
            var c = new StreamContent(new MemoryStream(Encoding.UTF7.GetBytes(@"{""message"":""Hello World!!""}")));

            c.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            c.Headers.ContentEncoding.Add("utf-7");
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                //Content = new HttpRequestStreamContent(new MemoryStream(Encoding.UTF7.GetBytes(@"{""message"":""Hello World!!""}")))
                //{
                //    ContentType = "application/json",
                //    CharacterSet = "charset=utf-7"
                //},
                Content = c,
                Timeout = 60000
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostXmlUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                //Content = new HttpRequestStringContent(@"<message>Hello World!!</message>")
                //{
                //    ContentType = "text/xml",
                //    CharacterSet = "charset=utf-8"
                //}
                Content = new StringContent(@"<message>Hello World!!</message>", Encoding.UTF7, "text/xml")
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostFormUrlEncodedUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                //Content = new HttpRequestStringContent(@"message=Hello%20World!!")
                //{
                //    ContentType = "application/x-www-form-urlencoded",
                //    CharacterSet = "charset=utf-7"
                //}
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("message", "hello world") } )
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                //content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostFormUrlEncodedUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                //Content = new HttpRequestStringContent(@"message=Hello%20World!!")
                //{
                //    ContentType = "application/x-www-form-urlencoded",
                //    CharacterSet = "charset=utf-8"
                //}
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("message", "hello world") })
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PostJsonUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                //Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                //{
                //    ContentType = "application/json",
                //    CharacterSet = "charset=utf-7"
                //}
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF7, "application/json")
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PutJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/put", HttpMethod.Put)
            {
                //Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                //{
                //    ContentType = "application/json",
                //    CharacterSet = "charset=utf-8"
                //}
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_Delete_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/delete", HttpMethod.Delete)
            {
                //Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                //{
                //    ContentType = "application/json",
                //    CharacterSet = "charset=utf-8"
                //}
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("Hello World");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_Get_TimeOut()
        {
            var request = new HttpRequest("http://httpbin.org/delay/5", HttpMethod.Get)
                          {
                              Timeout = 10
                          };

            using (var response = _sut.Send(request))
            {
                response.HttpStatusCode.ShouldBeNull();

                response.Exception.ShouldNotBeNull();
            }
        }


        //[Test]
        //public async void SendAsync_Get_TimeOut()
        //{
        //    var request = new HttpRequest("http://httpbin.org/delay/5", HttpMethod.Get)
        //    {
        //        Timeout = 10
        //    };

        //    using (var response = await _sut.SendAsync(request))
        //    {
        //        response.HttpStatusCode.ShouldBeNull();

        //        response.HttpExceptionStatus.ShouldBe(WebExceptionStatus.Timeout);

        //        response.Exception.ShouldNotBeNull();
        //    }
        //}

        [Test]
        public void Send_GetGzip_Ok()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpclient = new System.Net.Http.HttpClient(handler);

            var request = new HttpRequest("http://httpbin.org/gzip", HttpMethod.Get, httpclient);

            using (var response = _sut.Send(request))
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                content.ShouldContain("gzipped");

                response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                //response.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

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

        //    using (var response = _sut.Send(request))
        //    {
        //        var content = response.Content.Read();

        //        response.Content.IsString().ShouldBeTrue();

        //        content.ShouldContain("gzipped");

        //        response.Content.ContentType.ShouldBe("application/json");

        //        response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

        //        response.HttpExceptionStatus.ShouldBeNull();

        //        response.Exception.ShouldBeNull();
        //    }
        //}
    }
}
