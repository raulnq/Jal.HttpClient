using Castle.Windsor;

namespace Jal.HttpClient.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddHttpClient(this IWindsorContainer container)
        {
            container.Install(new HttpClientInstaller());
        }
    }
}