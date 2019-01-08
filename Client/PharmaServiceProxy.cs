
using ArVision.Common.Communication.Web;
using ArVision.Core.Logging;
using System;
using ArVision.Base.Shared;
using ArVision.Common.Data.DomainModel.Messages.Common;
using ArVision.Common.Communication.Web.Client;
using Newtonsoft.Json;
using System.Net;
using ArVision.Service.Pharma.Shared.DTO;
using ArVision.Service.Pharma.Shared;
using ArVision.Pharma.Shared.DataModels;
using System.Collections.Generic;

namespace ArVision.Service.Client
{
    public class PharmaServiceProxy : ArVisionBaseProxy, IPharmaService
    {
        private const string CLASS_NAME = nameof(PharmaServiceProxy);

        public PharmaServiceProxy(Session session, string serviceUrl) 
            : base(session, serviceUrl, PharmaServicePort.TCP_PORT, "")
        {
            
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);

        }
        public void Dispose() { }

        public ApiVersion GetVersion()
        {
            LogManager.Logger.Trace($"{LogManager.GetCurrentMethodName(CLASS_NAME)} invoked;");

            ApiVersion apiVersion = new ApiVersion();
            HttpWebClientResponse response;
             response = RestClient.Execute(RestSharpWebClientFactory.METHOD_GET, RestSharpWebClientFactory.REQUEST_FORMAT_JSON, PharmaServiceRoutes.ROUTE_GET_VERSION);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
                return JsonConvert.DeserializeObject<ApiVersion>(response.Content, settings);
            }
            return apiVersion;
        }


        public void Test(SampleInputData sampleInputData)
        {
            HttpWebClientResponse response = RestClient.Execute(RestSharpWebClientFactory.METHOD_POST, RestSharpWebClientFactory.REQUEST_FORMAT_JSON, PharmaServiceRoutes.ROUTE_TEST, sampleInputData);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw ParseExceptionFromHttpResponse(response.Content);
            }
        }
        public List<Juice> GetJuiceList()
        {
            return RestClient.Execute<List<Juice>>(RestSharpWebClientFactory.METHOD_POST, RestSharpWebClientFactory.REQUEST_FORMAT_JSON, PharmaServiceRoutes.ROUTE_GET_JUICE_LIST);
        }

        private Exception ParseExceptionFromHttpResponse(string jsonResponse)
        {
            dynamic parsedResponse = JsonConvert.DeserializeObject(jsonResponse);

            if (parsedResponse.ErrorCode != null)
            {
                string message = parsedResponse.Message.ToString();
            }

            throw ParseExceptionFromHttpResponse(jsonResponse);
        }

        public List<Patient> GetPatientList()
        {
            return RestClient.Execute<List<Patient>>(RestSharpWebClientFactory.METHOD_POST, RestSharpWebClientFactory.REQUEST_FORMAT_JSON, PharmaServiceRoutes.ROUTE_GET_PATIENT_LIST);
        }
    }
}
