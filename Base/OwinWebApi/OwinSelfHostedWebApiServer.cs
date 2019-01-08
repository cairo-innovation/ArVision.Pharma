using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Web.Http;

using Virtek.Core.Logging;
using Virtek.Core.Shared.Extensions;

using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using Swashbuckle.Application;
using Virtek.Base.OwinWebApi;

namespace Virtek.Base.OwinWebApi
{
    public sealed class OwinSelfHostedWebApiServer : ISelfHostedWebApiServer, IDisposable
    {
        private IDisposable webServer;
        private Container simpleInjectorContainer;
        private readonly int port;
        private readonly string serviceName;
        private const string PROTOCOL = "http://";
        private int disposed = 0;


        /// <summary>
        /// the port to listen on as well as the name of the service being hosted are passed during construction
        /// </summary>
        /// <param name="port"></param>
        /// <param name="serviceName"></param>
        public OwinSelfHostedWebApiServer(int port, string serviceName)
        {
            port.ThrowArgumentNullException("port");
            serviceName.ThrowArgumentNullException("serviceName");

            this.port = port;
            this.serviceName = serviceName;
        }

        /// <inheritdoc />
        /// <summary>
        /// implementation of the OWIN self hosted web api construct
        /// </summary>
        /// <param name="usePhysicalFileSystem">controls whether the physical file system and file system options code is executed during startup</param>
        /// <param name="registrationAction">reference to a container.Register method to add all required service interfaces and in process instances</param>
        /// <param name="assemblies">collection of controllers to include in the implemtation of the WebApi listener</param>
        public void Start(bool usePhysicalFileSystem, Action<Container> registrationAction, params Assembly[] assemblies)
        {
            string methodName = LogManager.GetCurrentMethodName(nameof(OwinSelfHostedWebApiServer));
            LogManager.Logger.Trace($"{methodName}; Starting...");

            simpleInjectorContainer?.Dispose();
            simpleInjectorContainer = new Container();
            simpleInjectorContainer.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            string BaseAddress = $"{PROTOCOL}*:{port}/";
            LogManager.Logger.Info($"{methodName}; {nameof(BaseAddress)}: {BaseAddress}");

            webServer = WebApp.Start(BaseAddress, (appBuilder) =>
            {
                HttpConfiguration config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();
                config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
                config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;

                var serializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
                var contractResolver = (DefaultContractResolver)serializerSettings.ContractResolver;
                contractResolver.IgnoreSerializableAttribute = true;

                config
                    .EnableSwagger(c => c.SingleApiVersion("v1", $"{serviceName} API"))
                    .EnableSwaggerUi();

                config.EnableCors();

                registrationAction(simpleInjectorContainer);

                simpleInjectorContainer.RegisterWebApiControllers(config, assemblies);

                simpleInjectorContainer.Verify();

                config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(simpleInjectorContainer);

                appBuilder.UseWebApi(config);

                if (usePhysicalFileSystem)
                {
                    PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem(@"./www");

                    FileServerOptions options = new FileServerOptions
                    {
                        EnableDefaultFiles = true,
                        FileSystem = physicalFileSystem,
                        StaticFileOptions =
                        {
                            FileSystem = physicalFileSystem,
                            ServeUnknownFileTypes = true
                        },
                        DefaultFilesOptions =
                        {
                            DefaultFileNames = new[]
                            {
                                "index.html"
                            }
                        }
                    };

                    appBuilder.UseFileServer(options);
                }
            });

            LogManager.Logger.Trace($"{methodName}; Started.");
        }

        public void Stop()
        {
            webServer?.Dispose();
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed, 1) == 1) return;
            Stop();
            simpleInjectorContainer?.Dispose();
        }

       
    }
}
