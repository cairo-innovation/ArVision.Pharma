using ArVision.Core.Logging;
using ArVision.Service.Pharma.Shared;
using ArVision.Service.Pharma.Shared.DTO;
using ArVision.Base.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using ArVision.Pharma.Shared.DataModels;
using ArVision.Pharma.DataAccess;
using System.Configuration;
using System.Web.Http.Cors;

namespace ArVision.Pharma.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController// ArVisionBaseApiController
    {
        private const string CLASS_NAME = nameof(ValuesController);
        //private readonly IPharmaService valuesService=new ArVision.Phar.PharmaService();
        //public ValuesController(IPharmaService valuesService, IServiceStatusProvider serviceStatusProvider) : base(serviceStatusProvider)
        //{
        //    this.valuesService = valuesService ?? throw new System.ArgumentNullException(nameof(valuesService));
        //}
        private static string dbpath = ConfigurationManager.AppSettings["DBPath"];
        private static string dbfilename = ConfigurationManager.AppSettings["DBFileName"];
        IPharmaRepository pharmaRepositoy = new PharmaRepository(System.Web.Hosting.HostingEnvironment.MapPath(dbpath), dbfilename);
        //// GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        //[Route(PharmaServiceRoutes.ROUTE_GET_PATIENT_WITH_RX)]
        //public HttpResponseMessage GetPatientWithRX(int? id)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    if (id == null)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }
        //    var patientrx = pharmaRepositoy.GetPatientWithRX(id.Value);
        //    response = Request.CreateResponse(HttpStatusCode.OK, patientrx, MediaTypeHeaderValue.Parse("application/json"));
        //    return response;
        //}
        [Route(PharmaServiceRoutes.ROUTE_GET_JUICE_LIST)]
        public List<Juices> GetJuiceList()
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.GetJuiceList(), false);
            return pharmaRepositoy.GetJuiceList();
        }

        [Route(PharmaServiceRoutes.ROUTE_GET_LIST)]
        public List<LookUpDto> GetList(string table)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.GetList(table), false);
            return pharmaRepositoy.GetList(table);
        }

        //[Route(PharmaServiceRoutes.ROUTE_GET_VERSION)]
        //[HttpGet]
        //public ApiVersion GetVersion()
        //{
        //    string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
        //    LogManager.Logger.Info($"{methodName} invoked;");
        //    //return Invoke(() => valuesService.GetVersion(), false);
        //    return pharmaRepositoy.GetVersion();
        //}
        [Route(PharmaServiceRoutes.ROUTE_TEST)]
        [HttpPost]
        public void Test(SampleInputData sampleInputData)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //Invoke(() => valuesService.Test(sampleInputData), false);
            //Invoke(() => valuesService.Test(sampleInputData), false);
        }
        //VisitDto AddVisitToPatient(VisitDto visit)
        [Route(PharmaServiceRoutes.ROUTE_ADD_VISIT_TO_PATIENT)]
        [HttpPost]
        public HttpResponseMessage AddVisit(VisitDto visit)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.AddPatient(patient), false);
            var r = pharmaRepositoy.AddVisitToPatient(visit);
            var response = Request.CreateResponse(HttpStatusCode.OK, r, MediaTypeHeaderValue.Parse("application/json"));
            return response;
        }
        [Route(PharmaServiceRoutes.ROUTE_ADD_PATIENT_RX)]
        [HttpPost]
        public HttpResponseMessage AddPatient(PatientDto patient)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.AddPatient(patient), false);
            var r = pharmaRepositoy.AddPatient(patient);
            var response = Request.CreateResponse(HttpStatusCode.OK, r, MediaTypeHeaderValue.Parse("application/json"));
            return response;
        }
        [Route(PharmaServiceRoutes.ROUTE_EDIT_PATIENT_RX)]
        [HttpPost]
        public PatientDto EditPatient(PatientDto patient)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.AddPatient(patient), false);
            return pharmaRepositoy.EditPatient(patient);
        }
        [Route(PharmaServiceRoutes.ROUTE_ADD_RX_TO_PATIENT)]
        [HttpPost]
        public RXDto AddRXToPatient(RXDto rx)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.AddRXToPatient(rx), false);
            return pharmaRepositoy.AddRXToPatient(rx);
        }
        [Route(PharmaServiceRoutes.ROUTE_GET_PATIENT_WITH_RX)]
        [HttpGet]
        public PatientDto GetPatientWithRX(int id)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);
            LogManager.Logger.Info($"{methodName} invoked;");
            //return Invoke(() => valuesService.GetPatientWithRX(id), false);
            return pharmaRepositoy.GetPatientWithRX(id);
        }
    }
}
