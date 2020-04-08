using System.Threading.Tasks;
using Jal.HttpClient.LightInject.Installer;
using Jal.HttpClient.Tests;
using LightInject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jal.HttpClient.LightInject.Tests
{
    [TestClass]
    public class HttpHandlerTests
    {
        private IHttpHandler _sut;

        [TestInitialize]
        public void Setup()
        { 
            var container = new ServiceContainer();

            container.AddHttpClient();

            _sut = container.GetInstance<IHttpHandler>();
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
    }
}
