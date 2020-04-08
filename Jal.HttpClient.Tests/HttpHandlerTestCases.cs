using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System;

namespace Jal.HttpClient.Tests
{
    public class HttpHandlerTestCases
    {
        public async Task Send_Get_Ok(IHttpHandler handler)
        {
            using (var response = await handler.SendAsync(new HttpRequest("http://httpbin.org/get", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("origin");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_GetWithHeaders_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Message.Headers.Add("Header1", "value");

            using (var response = await handler.SendAsync(request))
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

        public async Task Send_GetWithDuplicatedHeaders_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.Message.Headers.Add("Header1", "value");

            request.Message.Headers.Add("Header1", "value");

            using (var response = await handler.SendAsync(request))
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

        public async Task Send_GetWithQueryParameters_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/get", HttpMethod.Get);

            request.QueryParameters.Add(new HttpQueryParameter("parameter", "value"));

            using (var response = await handler.SendAsync(request))
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

        public async Task Send_GetXml_Ok(IHttpHandler handler)
        {
            using (var response = await handler.SendAsync(new HttpRequest("http://httpbin.org/xml", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("xml");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/xml");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_GetHtml_Ok(IHttpHandler handler)
        {
            using (var response = await handler.SendAsync(new HttpRequest("http://httpbin.org/html", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("<html>");

                response.Message.Content.Headers.ContentType.MediaType.ShouldContain("text/html");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_Get404_Ok(IHttpHandler handler)
        {
            using (var response = await handler.SendAsync(new HttpRequest("http://httpbin.org/status/404", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("text/html");

                response.Message.StatusCode.ShouldBe(HttpStatusCode.NotFound);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_Get500_Ok(IHttpHandler handler)
        {
            using (var response = await handler.SendAsync(new HttpRequest("http://httpbin.org/status/500", HttpMethod.Get)))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("text/html");

                response.Message.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostJsonUtf8_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostStreamJsonUtf8_Ok(IHttpHandler handler)
        {
            var streamcontent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(@"{""message"":""Hello World!!""}")));

            streamcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = streamcontent
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostStreamJsonUtf7_Ok(IHttpHandler handler)
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

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostXmlUtf8_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"<message>Hello World!!</message>", Encoding.UTF8, "text/xml")
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostFormUrlEncodedUtf7_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("message", "hello world") })
            };

            request.Message.Content.Headers.ContentType.CharSet = "utf-7";

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("hello world");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostFormUrlEncodedUtf8_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("message", "hello world") })
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("hello world");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PostJsonUtf7_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/post", HttpMethod.Post)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF7, "application/json")
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_PutJsonUtf8_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/put", HttpMethod.Put)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_Delete_Ok(IHttpHandler handler)
        {
            var request = new HttpRequest("http://httpbin.org/delete", HttpMethod.Delete)
            {
                Content = new StringContent(@"{""message"":""Hello World!!""}", Encoding.UTF8, "application/json")
            };

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("Hello World");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

        public async Task Send_Get_TimeOut(IHttpHandler handler)
        {
            var client = new System.Net.Http.HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(10)
            };
            var request = new HttpRequest("http://httpbin.org/delay/5", HttpMethod.Get, client)
            {
            };

            using (var response = await handler.SendAsync(request))
            {
                response.Message.ShouldBeNull();

                response.Exception.ShouldNotBeNull();
            }
        }

        public async Task Send_GetGzip_Ok(IHttpHandler handler)
        {
            HttpClientHandler dh = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpclient = new System.Net.Http.HttpClient(dh);

            var request = new HttpRequest("http://httpbin.org/gzip", HttpMethod.Get, httpclient);

            using (var response = await handler.SendAsync(request))
            {
                var content = await response.Message.Content.ReadAsStringAsync();

                content.ShouldContain("gzipped");

                response.Message.Content.Headers.ContentType.MediaType.ShouldBe("application/json");

                //response.Message.Content.Headers.ContentLength.Value.ShouldBeGreaterThan(0);

                response.Message.StatusCode.ShouldBe(HttpStatusCode.OK);

                response.Exception.ShouldBeNull();
            }
        }

    }
}
