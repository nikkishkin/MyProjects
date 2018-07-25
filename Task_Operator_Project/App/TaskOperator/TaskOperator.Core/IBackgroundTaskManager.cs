using System;

namespace TaskOperator.Core
{
    public interface IBackgroundTaskManager
    {
        void Run(Action taskDelegate); 
    } 
}
