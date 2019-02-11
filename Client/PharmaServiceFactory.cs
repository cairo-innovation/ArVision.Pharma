
using ArVision.Common.Data.DomainModel.Messages.Common;
using ArVision.Core.Shared.Extensions;

namespace ArVision.Service.Client
{
    public class PharmaServiceFactory : IPharmaServiceFactory
    {
        public PharmaServiceProxy GetPharmaServiceProxy(string serviceUrl, int TCP_PORT)
        {
            serviceUrl.ThrowArgumentNullException("serviceUrl");

            PharmaServiceProxy pharmaServiceProxy = new PharmaServiceProxy(Session.CreateDefaultSession(), serviceUrl, TCP_PORT);
            return pharmaServiceProxy;
        }

        public PharmaServiceProxy GetPharmaServiceProxy(Session session, string serviceUrl, int TCP_PORT)
        {
            session.ThrowArgumentNullException("session");
            serviceUrl.ThrowArgumentNullException("serviceUrl");

            PharmaServiceProxy pharmaServiceProxy = new PharmaServiceProxy(session, serviceUrl, TCP_PORT);
            return pharmaServiceProxy;
        }
    }
}
