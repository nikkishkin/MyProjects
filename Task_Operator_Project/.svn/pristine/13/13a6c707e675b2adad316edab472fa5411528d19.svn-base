using Ninject.Modules;
using TaskOperator.Logic.Interfaces;
using TaskOperator.Logic.Services;

namespace TaskOperator.Logic.IoC
{
    public class LogicModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITaskBlo>()
                .To<TaskBlo>()
                .InSingletonScope();

            Bind<IUserBlo>()
                .To<UserBlo>()
                .InSingletonScope();
        }
    }
}
