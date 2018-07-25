using Ninject.Modules;
using Ninject.Web.Common;
using TaskOperator.Core;
using TaskOperator.DAL.UnitOfWork;
using TaskOperator.Entities;

namespace TaskOperator.Web.IoC
{
    public class MvcModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EntityFrameworkUnitOfWork>()
                .InTransientScope();

            Bind<IDataProvider>().To<DataProvider>()
                .InSingletonScope();
        }
    }
}