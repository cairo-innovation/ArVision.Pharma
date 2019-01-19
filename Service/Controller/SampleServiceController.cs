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

        [Route(PharmaServiceRoutes.ROUTE_GET_JUICE_LIST)]
        public List<Juices> GetJuiceList()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.GetJuiceList(), false);
        }

        [Route(PharmaServiceRoutes.ROUTE_GET_LIST)]
        public List<LookUpDto> GetList(string table)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.GetList(table), false);
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
        [Route(PharmaServiceRoutes.ROUTE_ADD_PATIENT_RX)]
        [HttpPost]
        public PatientDto AddPatient(PatientDto patient)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.AddPatient(patient), false);
        }
        [Route(PharmaServiceRoutes.ROUTE_ADD_RX_TO_PATIENT)]
        [HttpPost]
        public RXDto AddRXToPatient(RXDto rx)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.AddRXToPatient(rx), false);
        }
        [Route(PharmaServiceRoutes.ROUTE_GET_PATIENT_WITH_RX)]
        [HttpGet]
        public PatientDto GetPatientWithRX(int id)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            return Invoke(() => sampleService.GetPatientWithRX(id), false);
        }
    }
}
