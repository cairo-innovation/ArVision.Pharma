using ArVision.Common.Data.DomainModel.Messages.Common;

namespace ArVision.Service.Client
{
    public interface IPharmaServiceFactory
    {
        PharmaServiceProxy GetPharmaServiceProxy(string serviceUrl, int TCP_PORT);

        PharmaServiceProxy GetPharmaServiceProxy(Session session, string serviceUrl, int TCP_PORT);
    }
}
