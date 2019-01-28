using System;

using Virtek.Base.Shared;

namespace Virtek.Base.ServiceLocationFoundation
{
    /// <summary>   Values that represent managed process exit codes. </summary>
    public enum ManagedProcessExitCode
    {
        /// <summary>The process exited normally.</summary>
        Ok = 0,
        /// <summary> The process intentionally exited for the explicit purpose of being restarted by its host.</summary>
        InternalRestart = 1000,
    }
    public interface IWebApiServiceLocationFoundation : IServiceStatusProvider, IDisposable
    {
        void Start();
        event Action<ManagedProcessExitCode> ServiceStopped;
        new string ServiceName { get; }
    }
}
