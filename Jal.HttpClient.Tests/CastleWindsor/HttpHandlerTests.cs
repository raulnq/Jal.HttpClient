using System.Threading.Tasks;
using Castle.Windsor;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jal.HttpClient.CastleWindsor.Tests
{
    [TestClass]
    public class HttpHandlerTests
    {
        private IHttpHandler _sut;

        [TestInitialize]
        public void Setup()
        { 
            var container = new WindsorContainer();

            container.AddHttpClient();

            _sut = container.Resolve<IHttpHandler>();
        }

        [TestMethod]
        public async Task Send_Get_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_Get_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_GetWithHeaders_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_GetWithHeaders_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_GetWithDuplicatedHeaders_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_GetWithDuplicatedHeaders_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_GetWithQueryParameters_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_GetWithQueryParameters_Ok(_sut);
        }
        [TestMethod]
        public async Task Send_GetXml_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_GetXml_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_GetHtml_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_GetHtml_Ok(_sut);
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
            var tests = new HttpHandlerTestCases();

            await tests.Send_Get404_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_Get500_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_Get500_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostJsonUtf8_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostJsonUtf8_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostStreamJsonUtf8_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostStreamJsonUtf8_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostStreamJsonUtf7_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostStreamJsonUtf7_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostXmlUtf8_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostXmlUtf8_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostFormUrlEncodedUtf7_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostFormUrlEncodedUtf7_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostFormUrlEncodedUtf8_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostFormUrlEncodedUtf8_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PostJsonUtf7_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PostJsonUtf7_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_PutJsonUtf8_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_PutJsonUtf8_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_Delete_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_Delete_Ok(_sut);
        }

        [TestMethod]
        public async Task Send_Get_TimeOut()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_Get_TimeOut(_sut);
        }

        [TestMethod]
        public async Task Send_GetGzip_Ok()
        {
            var tests = new HttpHandlerTestCases();

            await tests.Send_GetGzip_Ok(_sut);
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
