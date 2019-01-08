using System.Collections.Generic;
using System.Web.Http;
using ArVision.Base.Shared;
using ArVision.Core.Logging;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Service.Pharma.Shared;
using ArVision.Service.Pharma.Shared.DTO;

namespace ArVision.Service.Sample.Controller
{
    public class SampleServiceController : ArVisionBaseApiController, IPharmaService
    {
        private const string CLASS_NAME = nameof(SampleServiceController);
        private readonly IPharmaService sampleService;
        public SampleServiceController(IPharmaService sampleService, IServiceStatusProvider serviceStatusProvider) : base(serviceStatusProvider)
        {
            this.sampleService = sampleService ?? throw new System.ArgumentNullException(nameof(sampleService));
        }

        public List<Juice> GetJuiceList()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.GetJuiceList(), false);
        }

        public List<Patient> GetPatientList()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.GetPatientList(), false);
        }

        [Route(PharmaServiceRoutes.ROUTE_GET_VERSION)]
        [HttpGet]
        public ApiVersion GetVersion()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.GetVersion(), false);
        }
        [Route(PharmaServiceRoutes.ROUTE_TEST)]
        [HttpPost]
        public void Test(SampleInputData sampleInputData)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            Invoke(() => sampleService.Test(sampleInputData), false);
        }       
    }
}
