using System.Collections.Generic;
using TaskOperator.Entities;
using TaskOperator.Entities.Enums;

namespace TaskOperator.DAL.Interfaces
{
    public interface ITaskRepository
    {
        void AddTask(Task task);
        IEnumerable<Task> GetWorkerTasks(int workerId);

        IEnumerable<Task> GetAllTasks();

        Task GetTask(int id);

        void SetReadiness(int percentage, int taskId);

        void SaveManagerTask(int id, string name, string content, byte state, int workerId);

        bool IsWorker(int workerId, int taskId);

        User GetTaskWorker(Task task);

        User GetTaskWorker(int taskId);

        void SetPercentage(int id, int percentage);
    }
}
