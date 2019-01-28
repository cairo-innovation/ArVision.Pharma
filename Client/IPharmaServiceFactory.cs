using ArVision.Common.Data.DomainModel.Messages.Common;

namespace ArVision.Service.Client
{
    public interface IPharmaServiceFactory
    {
        PharmaServiceProxy GetPharmaServiceProxy(string serviceUrl);

        PharmaServiceProxy GetPharmaServiceProxy(Session session, string serviceUrl);
    }
}
