using System.Reflection;
using ArVision.Base.ServiceFoundation;
using ArVision.Core.Logging;
using ArVision.Core.Logging.Log4Net;
namespace ArVision.Service.SampleWindowsService.ConsoleHost
{
    public class Program
    {
        private const string CLASS_NAME = nameof(SampleWindowsService);

        private static int Main(string[] args)
        {
            int exitValue = 0;
                                   
            using (IWebApiServiceFoundation serviceInstance = CreateServiceInstance())
            {

                SetupLoggers(serviceInstance.ServiceName);
                IWebApiServiceRunner webApiServiceRunner = new WebApiServiceRunner();
                exitValue = webApiServiceRunner.Run(serviceInstance);
            }

            return exitValue;
        }
        private static IWebApiServiceFoundation CreateServiceInstance()
        {
            return new Pharma.PharmaService();
        }
        private static void SetupLoggers(string serviceName)
        {
            Log4NetFactory log4NetFactory = new Log4NetFactory();
            ILogging mainLogger = log4NetFactory.CreateLogger(Assembly.GetExecutingAssembly(), serviceName);
            LogManager.Logger = mainLogger;
        }
    }
}
