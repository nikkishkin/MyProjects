using System.ComponentModel;
using Ninject.Web.Mvc;
using TaskOperator.Core;
using TaskOperator.DAL.Interfaces;
using TaskOperator.DAL.IoC;
using TaskOperator.DAL.Repository;
using TaskOperator.DAL.UnitOfWork;
using TaskOperator.Entities;
using TaskOperator.Logic.Interfaces;
using TaskOperator.Logic.IoC;
using TaskOperator.Logic.Services;
using TaskOperator.Web.IoC;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TaskOperator.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TaskOperator.Web.App_Start.NinjectWebCommon), "Stop")]

namespace TaskOperator.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

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
            var kernel = new StandardKernel(new DalModule(), new LogicModule(), new IoC.MvcModule());
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
        }        
    }
}
