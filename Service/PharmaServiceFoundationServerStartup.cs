using System.Reflection;
using SimpleInjector;
using ArVision.Base.ServiceFoundation;
using ArVision.Service.Sample.Controller;
using ArVision.Service.Pharma.Shared;


namespace ArVision.Service.Pharma
{
    public class PharmaServiceFoundationServerStartup : WebApiFoundationServerStartup<IPharmaService>
    {
        public override string ServiceName => nameof(PharmaService);

        public override bool UsePhysicalFileSystem => false;
        public override System.Collections.Generic.IEnumerable<Assembly> ControllerAssemblies => new[]
        {
            typeof(SampleServiceController).Assembly
        };


        public override void RegisterServiceSpecificCompositionObjects(IPharmaService serviceInstance, Container container)
        {
            container.Register<IPharmaService>(() => serviceInstance, Lifestyle.Singleton);
        }

        public override int TcpPort => PharmaServicePort.TCP_PORT;

        public override int UdpPort => PharmaServicePort.UDP_PORT;

    }
}
