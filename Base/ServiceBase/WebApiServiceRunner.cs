using System.Threading;


namespace Virtek.Base.ServiceLocationFoundation
{
    public class WebApiServiceRunner : IWebApiServiceRunner
    {
        private ManualResetEvent stopGate = new ManualResetEvent(false);

        private int serviceExitValue;

        public string ServiceName { get; private set; }

        public int Run(IWebApiServiceLocationFoundation webApiServiceLocationFoundation)
        {
            ServiceName = webApiServiceLocationFoundation.ServiceName;

            webApiServiceLocationFoundation.ServiceStopped += Service_ServiceStopped;

            webApiServiceLocationFoundation.Start();
            stopGate.WaitOne();

            webApiServiceLocationFoundation.Dispose();
            return serviceExitValue;
        }

        private void Service_ServiceStopped(ManagedProcessExitCode managedProcessExitCode)
        {
            serviceExitValue = (int)managedProcessExitCode;
            stopGate.Set();
        }
    }
}
