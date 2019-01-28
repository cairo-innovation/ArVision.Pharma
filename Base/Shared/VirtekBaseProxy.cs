using System;

using Virtek.Common.Communication.Web;
using Virtek.Common.Communication.Web.Client;
using Virtek.Common.Data.DomainModel.Messages.Common;
using Virtek.Core.Logging;

using Newtonsoft.Json;

namespace Virtek.Base.Shared
{
    public class VirtekBaseProxy
    {
        private const string SESSION_OBJECT = @"Virtek.Session";
        private const string CLASS_NAME = nameof(VirtekBaseProxy);
        public IHttpWebClient RestClient;

        public VirtekBaseProxy(Session session, string serviceUrl, int port, string routePrefix)
        {
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);

            RestSharpWebClientFactory httpWebClientFactory = new RestSharpWebClientFactory();
            RestClient = httpWebClientFactory.Create();
            RestClient.UseRetryPolicy(HttpRetryPolicies.GeneralRetryPolicy());

            UriBuilder serviceUriBuilder = new UriBuilder(serviceUrl)
            {
                Port = port,
                Path = routePrefix
            };

            LogManager.Logger.Trace($"{methodName}; {nameof(serviceUriBuilder)}: ({serviceUriBuilder})");
            RestClient.Initialize(serviceUriBuilder.Uri);

            // add session to request header
            RestClient.AddHeader(SESSION_OBJECT, JsonConvert.SerializeObject(session));
        }
    }
}
