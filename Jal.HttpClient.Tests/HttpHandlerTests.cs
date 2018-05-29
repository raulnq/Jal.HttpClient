using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Logger.Installer;
using Jal.HttpClient.Model;
using NUnit.Framework;
using Shouldly;

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

            container.Register(Component.For<ILog>().Instance(log));

            container.Install(new HttpClientInstaller());

            container.Install(new HttpClienLoggertInstaller());

           _sut = container.Resolve<IHttpHandler>();
        }

        [Test]
        public void Send_Get_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/ip", HttpMethod.Get)))
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
        public async Task SendAsync_Get_Ok()
        {
            using (var response = await _sut.SendAsync(new HttpRequest("http://httpbin.org/get", HttpMethod.Get)))
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
        public void Send_GetWithHeaders_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Headers.Add(new HttpHeader("Header1", "value"));

            using (var response = _sut.Send(request))
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
        public void Send_GetWithDuplicatedHeaders_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Headers.Add(new HttpHeader("Header1", "value"));

            request.Headers.Add(new HttpHeader("Header1", "value"));

            using (var response = _sut.Send(request))
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
        public void Send_GetWithQueryParameters_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.QueryParameters.Add(new HttpQueryParameter("parameter", "value"));

            using (var response = _sut.Send(request))
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
        public void Send_GetXml_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/xml", HttpMethod.Get)))
            {
                var content = response.Content.Read();

                content.ShouldContain("xml");

                response.Content.IsString().ShouldBeTrue();

                response.Content.ContentType.ShouldBe("application/xml");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetHtml_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/html", HttpMethod.Get)))
            {
                var content = response.Content.Read();

                content.ShouldContain("<html>");

                response.Content.IsString().ShouldBeTrue();

                response.Content.ContentType.ShouldContain("text/html");

                response.Content.CharacterSet.ShouldBe("utf-8");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_GetImagePng_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/image/png", HttpMethod.Get)))
            {
                var image = Image.FromStream(response.Content.Stream);

                var same = ImageFormat.Png.Equals(image.RawFormat);

                same.ShouldBeTrue();

                response.Content.IsString().ShouldBeFalse();

                response.Content.ContentType.ShouldContain("image/png");

                response.Content.CharacterSet.ShouldBe("");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_Get404_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/status/404", HttpMethod.Get)))
            {
                response.Content.ContentType.ShouldContain("text/html");

                response.Content.CharacterSet.ShouldBe("utf-8");

                response.HttpStatusCode.ShouldBe(HttpStatusCode.NotFound);

                response.HttpExceptionStatus.ShouldBe(WebExceptionStatus.ProtocolError);

                response.HttpExceptionStatus.ShouldNotBeNull();
            }
        }

        [Test]
        public void Send_Get500_Ok()
        {
            using (var response = _sut.Send(new HttpRequest("http://httpbin.org/status/500", HttpMethod.Get)))
            {
                response.Content.ContentType.ShouldContain("text/html");

                response.Content.CharacterSet.ShouldBe("utf-8");

                response.HttpStatusCode.ShouldBe(HttpStatusCode.InternalServerError);

                response.HttpExceptionStatus.ShouldBe(WebExceptionStatus.ProtocolError);

                response.HttpExceptionStatus.ShouldNotBeNull();
            }
        }

        [Test]
        public void Send_PostJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=utf-8"
                }
            };

            using (var response = _sut.Send(request))
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
        public void Send_PostStreamJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStreamContent(new MemoryStream(Encoding.UTF8.GetBytes(@"{""message"":""Hello World!!""}")))
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=utf-8"
                }, Timeout = 60000
            };

            using (var response = _sut.Send(request))
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
        public void Send_PostStreamJsonUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStreamContent(new MemoryStream(Encoding.UTF7.GetBytes(@"{""message"":""Hello World!!""}")))
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=utf-7"
                },
                Timeout = 60000
            };

            using (var response = _sut.Send(request))
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
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStringContent(@"<message>Hello World!!</message>")
                {
                    ContentType = "text/xml",
                    CharacterSet = "charset=utf-8"
                }
            };

            using (var response = _sut.Send(request))
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
        public void Send_PostFormUrlEncodedUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStringContent(@"message=Hello%20World!!")
                {
                    ContentType = "application/x-www-form-urlencoded",
                    CharacterSet = "charset=utf-7"
                }
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                //content.ShouldContain("Hello World");

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
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStringContent(@"message=Hello%20World!!")
                {
                    ContentType = "application/x-www-form-urlencoded",
                    CharacterSet = "charset=utf-8"
                }
            };

            using (var response = _sut.Send(request))
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
        public void Send_PostJsonUtf7_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=utf-7"
                }
            };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                response.Content.ContentType.ShouldBe("application/json");

                response.Content.ContentLength.ShouldBeGreaterThan(0);

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

                response.Exception.ShouldBeNull();
            }
        }

        [Test]
        public void Send_PutJsonUtf8_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/put", HttpMethod.Put)
            {
                Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=utf-8"
                }
            };

            using (var response = _sut.Send(request))
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
        public void Send_Delete_Ok()
        {
            var request = new HttpRequest("http://httpbin.org/delete", HttpMethod.Delete)
            {
                Content = new HttpRequestStringContent(@"{""message"":""Hello World!!""}")
                {
                    ContentType = "application/json",
                    CharacterSet = "charset=utf-8"
                }
            };

            using (var response = _sut.Send(request))
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
        public void Send_Get_TimeOut()
        {
            var request = new HttpRequest("http://httpbin.org/delay/5", HttpMethod.Get)
                          {
                              Timeout = 10
                          };

            using (var response = _sut.Send(request))
            {
                response.HttpStatusCode.ShouldBeNull();

                response.HttpExceptionStatus.ShouldBe(WebExceptionStatus.Timeout);

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
            var request = new HttpRequest("http://httpbin.org/gzip", HttpMethod.Get)
                          {
                              Decompression = DecompressionMethods.GZip
                          };

            using (var response = _sut.Send(request))
            {
                var content = response.Content.Read();

                response.Content.IsString().ShouldBeTrue();

                content.ShouldContain("gzipped");

                response.Content.ContentType.ShouldBe("application/json");

                response.HttpStatusCode.ShouldBe(HttpStatusCode.OK);

                response.HttpExceptionStatus.ShouldBeNull();

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
