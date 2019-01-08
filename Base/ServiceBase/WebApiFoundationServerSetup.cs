using System.Collections.Generic;
using System.Reflection;
using Virtek.Common.Shared;
using Virtek.Base.Shared;
using SimpleInjector;

namespace Virtek.Base.ServiceLocationFoundation
{
    public abstract class WebApiFoundationServerSetup<TServiceInterface>
    {
        private const int HOST_COMMAND_PORT_OFFSET = 1;

        public abstract string ServiceName { get; }

        public abstract bool UsePhysicalFileSystem { get; }

        public abstract IEnumerable<Assembly> ControllerAssemblies { get; }

        public abstract RequestorServiceType RequestorServiceType { get; }

        public abstract int TcpPort { get; }                                    //This must be overridden by implementers as no default value makes sense here

        public int HostCommandTcpPort => TcpPort + HOST_COMMAND_PORT_OFFSET;    //This is used to control the service via messages from the host windows service

        public abstract int UdpPort { get;}                                    //This is used for multicasting by the service

        public virtual void RegisterDefinitions(TServiceInterface serviceInstance, Container container) { }
    }
}
