using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Virtek.Common.Services.Shared;
using Virtek.Core.Logging;

using Virtek.Base.OwinWebApi;

using Virtek.Common.ProofOfConcept.Data.Messages;


namespace Virtek.Base.ServiceLocationFoundation
{
    public abstract class WebApiServiceLocationFoundation<TServiceInterface, TWebApiServerStartup> : IWebApiServiceLocationFoundation
        where TWebApiServerStartup : WebApiFoundationServerStartup<TServiceInterface>, new()
    {
        public event Action<ManagedProcessExitCode> ServiceStopped;

        #region [Properties]

        // properties exposed to services
        protected string ApplicationFolder { get; }

        // external parameters provided
        private readonly TWebApiServerStartup webServerStartup;
        private OwinSelfHostedWebApiServer OwinSelfHostedWebApiServer { get; }

        // event object from controller
        
        private int disposed;

        private readonly object isServiceCompositionCompleteLock = new object();
        private readonly object isServiceConfigurationFirstPassCompleteLock = new object();
        private readonly object isServiceInitializeCompleteLock = new object();
        private readonly object isServiceShuttingDownLock = new object();
        private readonly object isServiceStartCompleteLock = new object();

        private bool isServiceConfigurationFirstPassComplete;
        private bool isServiceInitializeComplete;
        private bool isServiceShuttingDown;
        private bool isServiceStartComplete;

        private bool? consolePresent;
        private CancellationTokenSource readConsoleCancellationTokenSource;

        private ManagedProcessExitCode deferredManagedProcessExitCode;

        #endregion [Properties]

        #region [Constructors/Destructors]

        protected WebApiServiceLocationFoundation()
        {
            IsServiceInitializeComplete = false;
            log4net.LogicalThreadContext.Properties["session"] = "N/A";

        
            webServerStartup = new TWebApiServerStartup();

            disposed = 0;

            IsServiceConfigurationFirstPassComplete = false;
            IsServiceInitializeComplete = false;
            IsServiceShuttingDown = false;
            IsServiceStartComplete = false;


            // establish self hosted site object
            OwinSelfHostedWebApiServer = new OwinSelfHostedWebApiServer(webServerStartup.TcpPort, webServerStartup.ServiceName);
            ServiceState.Instance.DeferredServiceStopDue += ServiceState_DeferredServiceStopDue;
            consolePresent = null;
        }

        private void ServiceState_DeferredServiceStopDue()
        {
            Stop(deferredManagedProcessExitCode);
        }

        #endregion [Constructors/Destructors]

        ~WebApiServiceLocationFoundation()
        {
            Dispose(false);
        }

        #region [IServiceLocationFoundation]

        public void Start()
        {
            try
            {
                string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");

                if (IsServiceStartComplete == false)
                {
                    LogManager.Logger.Trace($"{methodName}; invoked");

                    // startup webApi server, udp listener, shutdown monitoring if not started already
                    Initialize();
                    IsServiceStartComplete = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.Error(ex.ToString());
                throw;
            }
        }

        public string ServiceName
        {
            get
            {
                return webServerStartup.ServiceName;
            }
        }

        private void Stop(ManagedProcessExitCode managedProcessExitCode)
        {
            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");
            LogManager.Logger.Trace($"{methodName}; invoked");

            if (IsServiceShuttingDown == false)
            {
                IsServiceShuttingDown = true;
                
                //[Signal to the ConsoleReading task that it should stop]
                if (readConsoleCancellationTokenSource != null)
                {
                    readConsoleCancellationTokenSource.Cancel();
                }

                ServiceState.Instance.PendingStop = false;
                ProcessStop();
                OnServiceStopped(managedProcessExitCode);
            }
            else
            {
                LogManager.Logger.Info($"{methodName}; skipped as service ({webServerStartup.ServiceName}) is shutting down");
            }
        }

       
        #endregion [IServiceLocationFoundation]

        #region [Private Methods]

        /// <summary>
        /// This is used to determine if the given service has received its initial configuration
        /// </summary>
        private bool IsServiceConfigurationFirstPassComplete
        {
            get
            {
                lock (isServiceConfigurationFirstPassCompleteLock)
                {
                    return isServiceConfigurationFirstPassComplete;
                }
            }
            set
            {
                lock (isServiceConfigurationFirstPassCompleteLock)
                {
                    isServiceConfigurationFirstPassComplete = value;
                }
            }
        }
        private void RegisterService()
        {
            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");
            LogManager.Logger.Trace($"{methodName}; invoked");

        }





        private void Initialize()
        {
            if (IsServiceInitializeComplete == false)
            {
                // add generic controller(s) to the controller collection
                List<Assembly> arrayOfControllers = webServerStartup.ControllerAssemblies.ToList();

                OwinSelfHostedWebApiServer.Start(webServerStartup.UsePhysicalFileSystem,
                                    (container) => webServerStartup.RegisterCompositionObjects(this, ServiceInstance, container),
                                    arrayOfControllers.ToArray());



                //[Iff a Console is present, read input from the console on a different thread (task)]
                if (IsConsolePresent)
                {
                    readConsoleCancellationTokenSource = new CancellationTokenSource();
                    CancellationToken readConsoleCancellationToken = readConsoleCancellationTokenSource.Token;
                    Task.Factory.StartNew(() => ReadInputFromConsole(readConsoleCancellationToken), readConsoleCancellationToken);
                }
                else
                {
                    readConsoleCancellationTokenSource = null;
                }
                IsServiceInitializeComplete = true;
            }
        }

        private void ReadInputFromConsole(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                string inputCommand = Console.ReadLine();
                if (inputCommand.Trim().ToUpper() == "STOP")
                {
                    HandleExternalStopRequest();
                    break;
                }
            }
        }

        private void HandleExternalStopRequest()
        {
            HandleStop(ManagedProcessExitCode.Ok);
        }

       
       
       
        private void ResetRequired(object sender, EventArgs e)
        {
            HandleResetRequired();
        }

        private void OnServiceStopped(ManagedProcessExitCode managedProcessExitCode)
        {
            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");
            LogManager.Logger.Info($"{methodName}; Firing {nameof(ServiceStopped)} event");

            ServiceStopped?.Invoke(managedProcessExitCode);
        }


        
        private void HandleResetRequired()
        {
            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");
            LogManager.Logger.Trace($"{methodName}; invoked");

            if (IsServiceShuttingDown == false)
            {

                //[Check if the descendant supports Resetting.  If so, invoke the ProcessReset method, otherwise, invoke the HandleStop method]
                if (ServiceSupportsReset)
                {
                    LogManager.Logger.Info($"{methodName}; {webServerStartup.ServiceName} supports Reset, invoking method ({nameof(ProcessReset)})");
                    ProcessReset();
                }
                else
                {
                    LogManager.Logger.Info($"{methodName}; {webServerStartup.ServiceName} does not support Reset, invoking method ({nameof(HandleStop)})");
                    HandleStop(ManagedProcessExitCode.InternalRestart);
                }
            }
            else
            {
                LogManager.Logger.Info($"{methodName}; skipped as Service is shutting down");
            }
        }

        private void HandleStop(ManagedProcessExitCode managedProcessExitCode)
        {
            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");
            LogManager.Logger.Trace($"{methodName}; invoked");

            if (IsServiceShuttingDown == false)
            {
                if (ServiceState.Instance.IsBusy == false)
                {
                    LogManager.Logger.Info($"{methodName}; {webServerStartup.ServiceName} is NOT busy");
                    Stop(managedProcessExitCode);
                }
                else
                {
                    LogManager.Logger.Info($"{methodName}; {webServerStartup.ServiceName} is busy, Setting {nameof(ServiceState.Instance.PendingStop)} to True");
                    deferredManagedProcessExitCode = managedProcessExitCode;        //[Set the exit code to be used when the deferred stop is ultimately executed]
                    ServiceState.Instance.PendingStop = true;
                }
            }
            else
            {
                LogManager.Logger.Info($"{methodName}; skipped as Service is shutting down");
            }
        }

        private bool IsConsolePresent
        {
            get
            {
                if (consolePresent == null)
                {
                    consolePresent = true;
                    try
                    {
                        int windowHeight = Console.WindowHeight;
                    }
                    catch
                    {
                        consolePresent = false;
                    }
                }
                return consolePresent.Value;
            }
        }
        #endregion [Private Methods]

        #region [Protected Methods]

       
        protected bool LogError(Exception ex, [CallerMemberName] string callerMemberName = "")
        {
            LogManager.Logger.Error(ex, $"{webServerStartup.ServiceName}.{callerMemberName}; An unexpected error has occurred.");
            return false;
        }

        #endregion [Protected Methods]

        #region [Abstract Methods/Properties]


        /// <summary>
        /// This property requires implementers (descendant services) to return their ServiceInterface type
        /// </summary>
        protected abstract TServiceInterface ServiceInstance { get; }

        /// <summary>
        /// This property requires implementers (descendant services) to indicate whether they support reset or whether they must be restarted
        /// <remarks>
        /// This wis typically used when a 'significant' configuration change is received to determine whether the service
        /// can reset itself or whether it will be asked to shutdown.
        /// </remarks>
        /// </summary>
        protected abstract bool ServiceSupportsReset { get; }

        #endregion [Abstract Methods/Properties]

        #region [Virtual Methods/Properties]



        /// <summary>
        /// This method is used to allow implementers (descendant services) to perform a reset (for example following a 'significant' configuration change)
        /// <remarks>
        /// This method is only invoked by the base class (this class) if the service indicates that it supports reset (via the ServiceSupportsReset property)
        /// It is implemented as a virtual method as it is possible that descendant classes will not need to perform special processing
        /// </remarks>
        /// </summary>
        protected virtual void ProcessReset()
        {
        }

        /// <summary>
        /// This method is used to allow implementers (descendant services) to perform specific processing when the service is about to be stopped
        /// <remarks>
        /// It is implemented as a virtual method as it is possible that descendant classes will not need to perform special processing prior to being stopped
        /// </remarks>
        /// </summary>
        protected virtual void ProcessStop()
        {
        }

        #endregion [Virtual Methods/Properties]

        #region [IServiceStatusProvider implementation]



        /// <summary>
        /// This is used to determine if the given service has been initialized to ensure that initialization only occurs once
        /// </summary>
        public bool IsServiceInitializeComplete
        {
            get
            {
                lock (isServiceInitializeCompleteLock)
                {
                    return isServiceInitializeComplete;
                }
            }
            set
            {
                lock (isServiceInitializeCompleteLock)
                {
                    isServiceInitializeComplete = value;
                }
            }
        }

        
        /// <summary>
        /// This is used to determine if the given service has completed its startup
        /// </summary>
        public bool IsServiceStartComplete
        {
            get
            {
                lock (isServiceStartCompleteLock)
                {
                    return isServiceStartComplete;
                }
            }
            set
            {
                lock (isServiceStartCompleteLock)
                {
                    isServiceStartComplete = value;
                }
            }
        }

        /// <summary>
        /// This is used to determine if the given service is in the process of shutting down
        /// </summary>
        public bool IsServiceShuttingDown
        {
            get
            {
                lock (isServiceShuttingDownLock)
                {
                    return isServiceShuttingDown;
                }
            }
            set
            {
                lock (isServiceShuttingDownLock)
                {
                    isServiceShuttingDown = value;
                }
            }
        }


        #endregion [IServiceStatusProvider implementation]

        #region [Dispose]

        public void Dispose()
        {
            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");

            LogManager.Logger.Trace($"{methodName}; invoked");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposed, 1) == 1)
            {
                return;
            }

            string methodName = LogManager.GetCurrentMethodName($"BASE: {webServerStartup.ServiceName}");
            LogManager.Logger.Trace($"{methodName}; invoked");

            if (disposing)
            {
                OwinSelfHostedWebApiServer?.Dispose();
            }
        }
        #endregion [Dispose]
    }
}
