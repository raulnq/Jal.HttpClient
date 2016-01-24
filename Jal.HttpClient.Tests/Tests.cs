using Castle.Windsor;
using Jal.HttpClient.Fluent;
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

            var response = httpclient.Send(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));
        }

        [Test]
        public void SendAsync_WithHttpMethodGetHttpContentTypeForm_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var httpclient = container.Resolve<IHttpHandler>();

            var task = httpclient.SendAsync(new HttpRequest("https://github.com/raulnq", HttpMethod.Get));

            var response = task.Result;
        }


        [Test]
        public void Send2_WithHttpMethodGetHttpContentTypeForm_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var builder = container.Resolve<IHttpHandlerBuilder>();

            var response = builder.For("https://github.com/raulnq").Send();

        }

        [Test]
        public void Send3_WithHttpMethodGetHttpContentTypeForm_Ok()
        {
            var container = new WindsorContainer();

            container.Install(new HttpClientInstaller());

            var builder = container.Resolve<IHttpHandlerBuilder>();

            var response = builder.For("https://github.com/raulnq").Get().Send();

        }
    }
}
