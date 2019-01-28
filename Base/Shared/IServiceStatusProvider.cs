namespace Virtek.Base.Shared
{
    public interface IServiceStatusProvider
    {
        bool IsServiceInitializeComplete { get; }
        bool IsServiceShuttingDown { get; }
        bool IsServiceStartComplete { get; }

        string ServiceName { get; }

        //TODO: (KA) Add these to this interface for use with the VirtekBaseApiController (may be able to just have the IsServiceBusy and make it get/set)
        //bool IsServiceBusy { get; }
        //void IncrementServiceBusyState();
        //void DecrementServiceBusyState();
    }
}
