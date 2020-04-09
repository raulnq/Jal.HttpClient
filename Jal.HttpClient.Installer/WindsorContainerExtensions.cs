using Castle.Windsor;
using System;

namespace Jal.HttpClient.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddHttpClient(this IWindsorContainer container, Action<IWindsorContainer> action = null)
        {
            container.Install(new HttpClientInstaller());

            if (action != null)
            {
                action(container);
            }
        }

        public static IHttpFluentHandler GetHttpClient(this IWindsorContainer container)
        {
            return container.Resolve<IHttpFluentHandler>();
        }
    }
}