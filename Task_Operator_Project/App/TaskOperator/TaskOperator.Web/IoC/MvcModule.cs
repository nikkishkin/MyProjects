using Ninject.Modules;
using TaskOperator.Core;
using TaskOperator.DAL.UnitOfWork;
using TaskOperator.Web.TaskManagement;
using TaskOperator.Web.UnitOfWork;

namespace TaskOperator.Web.IoC
{
    public class MvcModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EntityFrameworkUnitOfWork>()
                .InTransientScope();

            Bind<IUofProvider>().To<HttpContextUofProvider>()
                .InSingletonScope();

            Bind<IBackgroundTaskManager>().To<AspNetBackgroundTaskManager>()
                .InSingletonScope();
        }
    }
}