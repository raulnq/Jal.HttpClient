using Castle.Windsor;

namespace Jal.HttpClient.Serilog.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddSerilogForHttpClient(this IWindsorContainer container)
        {
            container.Install(new HttpClientSerilogInstaller());
        }
    }
}