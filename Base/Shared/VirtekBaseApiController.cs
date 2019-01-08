using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.Http.Controllers;

using Virtek.Common.Data.DomainModel.Messages.Common;
using Virtek.Common.Services;
using Virtek.Core.Logging;

using Newtonsoft.Json;
using Virtek.Base.Shared;

namespace Virtek.Base.Shared
{
    public abstract class VirtekBaseApiController : ApiController
    {
        private const string SESSION_OBJECT = @"Virtek.Session";
        private readonly IServiceStatusProvider serviceStatusProvider;

        protected VirtekBaseApiController(IServiceStatusProvider serviceStatusProvider)
        {
            this.serviceStatusProvider = serviceStatusProvider;
        }

        #region Methods

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            if (Request != null)
            {
                var session = GetSessionObject(Request);
                var sessionId = session.SessionId;

                // the value from the property bag can be exposed in the log by adding the %property{session} mask in the conversionPattern in app.config / <log4net>
                LogManager.LogicalThreadContext["session"] = sessionId;
            }
        }

        protected T Invoke<T>(Func<T> invocationTarget, bool requiresServiceInitialization = true, [CallerMemberName] string callerMemberName = "")
        {
            var result = default(T);
            Invoke(() => { result = invocationTarget.Invoke(); }, requiresServiceInitialization, callerMemberName);
            return result;
        }

        protected void Invoke(Action invocationTarget, bool requiresServiceInitialization = true, [CallerMemberName] string callerMemberName = "")
        {
            try
            {
                if (requiresServiceInitialization)
                {
                    VerifyThatServiceCanProcessRequest();
                }
                invocationTarget.Invoke();
            }
            catch (Exception ex) when (LogError(ex, callerMemberName)) // This will cause all exceptions to be logged
            {
                /* This catch block will never be entered */
            }
            catch (ServiceNotInitializedException ex)
            {
                throw CreateHttpResponseException(HttpStatusCode.ServiceUnavailable, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw CreateHttpResponseException(HttpStatusCode.BadRequest, ex);
            }
            catch (ArgumentException ex)
            {
                throw CreateHttpResponseException(HttpStatusCode.BadRequest, ex);
            }
            catch (Exception ex)
            {
                throw CreateHttpResponseException(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected bool LogError(Exception ex, [CallerMemberName] string callerMemberName = "")
        {
            // Get the method name this way, to reflect the name of the derived controller, and the method name of the caller
            string methodName = $@"{GetType().Name}.{callerMemberName}";
            LogManager.Logger.Error(ex, $@"{methodName}; An unexpected error has occurred.");
            return false;
        }

        private void VerifyThatServiceCanProcessRequest()
        {
           if (serviceStatusProvider.IsServiceShuttingDown)
            {
                throw new ServiceNotInitializedException($@"{serviceStatusProvider.ServiceName} is in the process of shutting down");
            }
        }

        private void TrySetServiceBusy()
        {
            string methodName = LogManager.GetCurrentMethodName(GetType().Name);

            //TODO: (KA) Add the busy getter and increment/decrement methods to the IServiceStatusProvider interface
            /*
            if (serviceStatusProvider.IsServiceBusy == false)
            {
                LogManager.Logger.Trace($"{methodName}; service {serviceStatusProvider.ServiceName} is not currently busy, setting service to busy");
                serviceStatusProvider.IncrementServiceBusyState();
            }
            */
        }

        private void TrySetServiceNotBusy()
        {
            string methodName = LogManager.GetCurrentMethodName(GetType().Name);

            //TODO: (KA) Add the busy getter and increment/decrement methods to the IServiceStatusProvider interface
            /*
            if (serviceStatusProvider.IsServiceBusy == true)
            {
                LogManager.Logger.Trace($"{methodName}; service {serviceStatusProvider.ServiceName} is currently busy, setting service to not busy");
                serviceStatusProvider.DecrementServiceBusyState();
            }
            */
        }

        private static Session GetSessionObject(HttpRequestMessage request)
        {
            var resultSession = Session.CreateDefaultSession();

            request.Headers.TryGetValues(SESSION_OBJECT, out IEnumerable<string> sessionObject);

            if (sessionObject != null)
            {
                var session = sessionObject.First();
                if (!string.IsNullOrWhiteSpace(session))
                {
                    resultSession = JsonConvert.DeserializeObject<Session>(session);
                }
            }
            return resultSession;
        }

        private HttpResponseException CreateHttpResponseException(HttpStatusCode statusCode, Exception ex)
        {
            return new HttpResponseException(Request.CreateErrorResponse(statusCode, ex));
        }

        #endregion Methods
    }
}