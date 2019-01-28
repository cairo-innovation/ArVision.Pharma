using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;
using Virtek.Base.Shared;

namespace Virtek.Base.ServiceLocationFoundation
{
    public abstract class WebApiFoundationServerStartup<TServiceInterface>
    {
        private const int HOST_COMMAND_PORT_OFFSET = 1;

        public abstract string ServiceName { get; }

        public abstract bool UsePhysicalFileSystem { get; }

        public abstract IEnumerable<Assembly> ControllerAssemblies { get; }

        public abstract int TcpPort { get; }                                    //This must be overridden by implementers as no default value makes sense here

        public int HostCommandTcpPort => TcpPort + HOST_COMMAND_PORT_OFFSET;    //This is used to control the service via messages from the host windows service

        public void RegisterCompositionObjects(IServiceStatusProvider serviceStatusProvider, TServiceInterface serviceInstance, Container simpleInjectorContainer)
        {
            simpleInjectorContainer.Register<IServiceStatusProvider>(() => serviceStatusProvider, Lifestyle.Singleton);
            RegisterServiceSpecificCompositionObjects(serviceInstance, simpleInjectorContainer);
        }
        public virtual void RegisterServiceSpecificCompositionObjects(TServiceInterface serviceInstance, Container simpleInjectorContainer)
        {
            //[Descendants use overides of this method to register their service-specific components]
        }
        public abstract int UdpPort { get;}                                    //This is used for multicasting by the service
    }
}
