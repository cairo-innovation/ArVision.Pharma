namespace Virtek.Base.ServiceLocationFoundation
{
    public interface IWebApiServiceRunner
    {
        int Run(IWebApiServiceLocationFoundation webApiServiceLocationFoundation);
        string ServiceName { get; }
    }
}
