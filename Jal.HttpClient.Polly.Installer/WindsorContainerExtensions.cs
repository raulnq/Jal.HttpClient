using Castle.Windsor;

namespace Jal.HttpClient.Polly.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddPollyForHttpClient(this IWindsorContainer container)
        {
            container.Install(new HttpClientPollyInstaller());
        }
    }
}