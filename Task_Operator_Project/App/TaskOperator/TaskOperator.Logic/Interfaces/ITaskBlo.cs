using System.Collections.Generic;
using TaskOperator.Entities;

namespace TaskOperator.Logic.Interfaces
{
    public interface ITaskBlo
    {
        void CreateTask(string name);

        Task GetTask(int id);

        bool IsWorker(int workerId, int taskId);

        void SaveManagerTask(int id, string name, string content, byte state, int workerId);

        User GetTaskWorker(Task task);

        User GetTaskWorker(int taskId);

        void SetPercentage(int id, int percentage);

        //bool ManagerPageExists(int pageNumber, int pageSize);

        IEnumerable<Task> GetManagerTasks(int pageNumber, int pageSize);

        //bool WorkerPageExists(int pageNumber, int pageSize, int workerId);

        IEnumerable<Task> GetWorkerTasks(int pageNumber, int pageSize, int workerId);

        int GetAllTasksCount();

        int GetWorkerTasksCount(int workerId);

        void DeleteTask(int id);
    }
}
