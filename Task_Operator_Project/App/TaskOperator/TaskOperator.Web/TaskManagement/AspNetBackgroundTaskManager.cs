using System;
using System.Web.Hosting;
using TaskOperator.Core;

namespace TaskOperator.Web.TaskManagement
{
    public class AspNetBackgroundTaskManager: IBackgroundTaskManager
    {
        public void Run(Action taskAction)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct => taskAction());
        }
    }
}