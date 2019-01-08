using System;
using System.Reflection;
using SimpleInjector;

namespace Virtek.Base.OwinWebApi
{
    public interface ISelfHostedWebApiServer
    {
        /// <summary>
        /// implementation of the OWIN self hosted web api construct
        /// </summary>
        /// <param name="UsePhysicalFileSystem">controls whether the physical file system and file system options code is executed during startup</param>
        /// <param name="registrationAction">reference to a container.Register method to add all required service interfaces and in process instances</param>
        /// <param name="assemblies">collection of controllers to include in the implemtation of the WebApi listener</param>
        void Start(bool UsePhysicalFileSystem, Action<Container> registrationAction, params Assembly[] assemblies);

        void Stop();
    }
}