using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using AutoMapper;
using HometasksMonitoringPanel;
using HometasksMonitoringPanel.Configs;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using HometasksMonitoringPanel.Helpers;
using HometasksMonitoringPanel.Providers;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace HometasksMonitoringPanel
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var tokenKey = ConfigurationManager.AppSettings["gitHubTokenKey"] ?? "hometaskmonitoring:GithubToken";
            var baseApiUrl = ConfigurationManager.AppSettings["gitHubBaseApiUrl"] ?? "https://api.github.com";
            var baseUrl = ConfigurationManager.AppSettings["gitHubBaseUrl"] ?? "https://github.com";
            var token = Environment.GetEnvironmentVariable(tokenKey, EnvironmentVariableTarget.Machine);
            if (token.Empty())
            {
                throw new ArgumentException("Github token is not found, please specify the token in machines variables {0}".Fmt(token));
            }

            Assembly.GetCallingAssembly().GetTypes()
                                         .Where(t => typeof(Profile).IsAssignableFrom(t))
                                         .ToList()
                                         .ForEach(t =>
                                         {
                                             Mapper.AddProfile((Profile)bootstrapper.Kernel.Get(t));
                                         });

            Mapper.Initialize(config =>
            {
                config.ConstructServicesUsing(t => bootstrapper.Kernel.Get(t));
            });

            kernel.Bind<GithubConfig>().ToConstant(new GithubConfig(baseApiUrl, baseUrl, token));
            kernel.Bind<IHometasksProvider>().To<HometasksProvider>();
            kernel.Bind<IIssuesProvider>().To<GithubIssuesProvider>();
            kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
            kernel.Bind<ICouchProvider>().To<CouchProvider>();
        }
    }
}
