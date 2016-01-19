using Castle.Windsor;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Model;
using NUnit.Framework;

namespace Jal.HttpClient.Tests
{
    [TestFixture]
    public class Tests
    {


        [Test]
        public void Send_WithHttpMethodGetHttpContentTypeForm_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var response = httpclient.Send(new HttpRequest("https://github.com/raulnq", HttpMethod.Get, HttpContentType.Form));
        }

        [Test]
        public void SendAsync_WithHttpMethodGetHttpContentTypeForm_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var task = httpclient.SendAsync(new HttpRequest("https://github.com/raulnq", HttpMethod.Get, HttpContentType.Form));

            var response = task.Result;
        }
    }
}
