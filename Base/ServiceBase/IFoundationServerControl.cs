using System;

namespace Virtek.Base.ServiceLocationFoundation
{
    public interface IFoundationServerControl
    {
        event EventHandler<EventArgs> ShutdownRequested;

        void StartMonitoringForShutdownRequest();
    }
}