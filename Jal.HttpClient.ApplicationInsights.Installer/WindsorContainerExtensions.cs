using Castle.Windsor;

namespace Jal.HttpClient.ApplicationInsights.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddApplicationInsightsForHttpClient(this IWindsorContainer container, string applicationname)
        {
            container.Install(new HttpClientApplicationInsightstInstaller(applicationname));
        }
    }
}