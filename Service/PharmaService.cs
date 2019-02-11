using ArVision.Core.Logging;
using ArVision.Service.Pharma.Shared.DTO;
using ArVision.Service.Pharma.Shared;
using ArVision.Base.ServiceFoundation;
using System.Threading;
using System.Reflection;
using ArVision.Pharma.Shared.DataModels;
using System.Collections.Generic;
using ArVision.Pharma.DataAccess;

namespace ArVision.Service.Pharma
{
    public class PharmaService : WebApiServiceFoundation<IPharmaService, PharmaServiceFoundationServerStartup>, IPharmaService
    {
        private const string CLASS_NAME = nameof(PharmaService);
        public const string APPLICATION_NAME = "SampleService";
        //private const string DATA_BASE_FOLDER_PATH = ".\\Database\\";

        IPharmaRepository pharmaRepositoy;

        private int disposed;
        public PharmaService()
        {
            disposed = 0;
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(nameof(PharmaService))};");
            pharmaRepositoy = new PharmaRepository();

        }

        ~PharmaService()
        {
            Dispose(false);
        }

        #region [Abstract Method/Property Overrides]
        protected override IPharmaService ServiceInstance => this;

        protected override void ProcessReset()
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(CLASS_NAME)} invoked;");            
            
        }
        protected override bool ServiceSupportsReset
        {
            get
            {
                return true;
            }
        }
        #endregion [Abstract Method/Property Overrides]
        #region [Dispose]
        protected override void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposed, 1) == 1) return;

            LogManager.Logger.Trace("Dispose Invoked");

            base.Dispose(disposing);
            // Free any unmanaged objects here.            
        }

       
        #endregion [Dispose]
        #region [ISampleService implementation]
        public ApiVersion GetVersion()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            ApiVersion apiVersionDto = new ApiVersion(version);
            LogManager.Logger.Trace($"{methodName} invoked; returning {nameof(apiVersionDto)}: ({apiVersionDto})");

            return apiVersionDto;
        }
        public void Test(SampleInputData sampleInputData)
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(nameof(PharmaService))}; sampleInputData: [{sampleInputData}]");

        }
        public PatientDto EditPatient(PatientDto patient)
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(nameof(PharmaService))}; patient: [{patient}]");
            return pharmaRepositoy.EditPatient(patient);
        }
        public PatientDto AddPatient(PatientDto patient)
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(nameof(PharmaService))}; patient: [{patient}]");
            return pharmaRepositoy.AddPatient(patient);
        }
        public RXDto AddRXToPatient(RXDto rx)
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(nameof(PharmaService))}; RX: [{rx}]");
            return pharmaRepositoy.AddRXToPatient(rx);
        }
        public List<Juices> GetJuiceList()
        {
            return pharmaRepositoy.GetJuiceList();
        }

        public List<LookUpDto> GetList(string table)
        {
            return pharmaRepositoy.GetList(table);
        }
        public PatientDto GetPatientWithRX(int id)
        {
            return pharmaRepositoy.GetPatientWithRX(id);
        }
        public List<Patient> GetPatientList()
        {
            return pharmaRepositoy.GetPatientList();
        }
        #endregion [ISampleService implementation]

    }
}
