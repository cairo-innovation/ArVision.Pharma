using System;
using Virtek.Common.Services.Shared;

namespace Virtek.Base.ServiceLocationFoundation
{
    /// <summary>
    /// A wrapper class to handle the service busy flag
    /// </summary>
    public class ServiceBusyIndicator : IDisposable
    {
        public ServiceBusyIndicator()
        {
            ServiceState.Instance.IncrementBusyState();
        }

        public void Dispose()
        {
            ServiceState.Instance.DecrementBusyState();
        }
    }
}
