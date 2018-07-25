using Ninject.Modules;
using TaskOperator.DAL.Interfaces;
using TaskOperator.DAL.Repository;

namespace TaskOperator.DAL.IoC
{
    public class DalModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserRepository>()
                .To<UserRepository>()
                .InSingletonScope();

            Bind<ITaskRepository>()
                .To<TaskRepository>()
                .InSingletonScope();
        }
    }
}
