using Castle.Windsor;

namespace Jal.HttpClient.Common.Logging.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddCommonLoggingForHttpClient(this IWindsorContainer container)
        {
            container.Install(new HttpClientCommonLoggingInstaller());
        }
    }
}