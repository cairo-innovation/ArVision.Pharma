
using ArVision.Common.Data.DomainModel.Messages.Common;
using ArVision.Core.Shared.Extensions;

namespace ArVision.Service.Client
{
    public class PharmaServiceFactory : IPharmaServiceFactory
    {
        public PharmaServiceProxy GetPharmaServiceProxy(string serviceUrl)
        {
            serviceUrl.ThrowArgumentNullException("serviceUrl");

            PharmaServiceProxy pharmaServiceProxy = new PharmaServiceProxy(Session.CreateDefaultSession(), serviceUrl);
            return pharmaServiceProxy;
        }

        public PharmaServiceProxy GetPharmaServiceProxy(Session session, string serviceUrl)
        {
            session.ThrowArgumentNullException("session");
            serviceUrl.ThrowArgumentNullException("serviceUrl");

            PharmaServiceProxy pharmaServiceProxy = new PharmaServiceProxy(session, serviceUrl);
            return pharmaServiceProxy;
        }
    }
}
