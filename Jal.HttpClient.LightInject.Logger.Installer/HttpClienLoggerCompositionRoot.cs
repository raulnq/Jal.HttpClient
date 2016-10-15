using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.HttpClient.Interface;
using Jal.HttpClient.Logger;
using LightInject;

namespace Jal.HttpClient.LightInject.Logger.Installer
{
    public class HttpClienLoggerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IHttpInterceptor, HttpLogger>(new PerContainerLifetime());
        }
    }
}
