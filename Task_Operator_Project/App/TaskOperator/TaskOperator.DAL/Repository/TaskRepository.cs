using System.Collections.Generic;
using System.Linq;
using TaskOperator.Core;
using TaskOperator.DAL.Interfaces;
using TaskOperator.DAL.UnitOfWork;
using TaskOperator.Entities;
using TaskOperator.Entities.Enums;

namespace TaskOperator.DAL.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IUofProvider _dataProvider;

        public TaskRepository(IUofProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        private TaskOperatorEntities GetContext()
        {
            return ((IEntityFrameworkUnitOfWork) (_dataProvider.GetUnitOfWork())).GetContext();
        }

        public void AddTask(Task task)
        {
            TaskOperatorEntities context = GetContext();
            context.Task.Add(task);
            context.SaveChanges();
        }

        public int GetAllTasksCount()
        {
            return GetContext().Task.Count();
        }

        public int GetWorkerTasksCount(int workerId)
        {
            return GetContext().Task.Count(t => t.WorkerId == workerId);
        }

        //public bool ManagerPageExists(int pageNumber, int pageSize)
        //{
        //    return GetContext().Task.Count() > pageNumber * pageSize;
        //}

        public IEnumerable<Task> GetManagerTasks(int pageNumber, int pageSize)
        {
            return GetContext().Task.OrderByDescending(t => t.Date).Skip(pageNumber*pageSize).Take(pageSize).ToArray();
        }

        //public bool WorkerPageExists(int pageNumber, int pageSize, int workerId)
        //{
        //    return GetContext().Task.Count(t => t.WorkerId == workerId) > pageNumber * pageSize;
        //}

        public IEnumerable<Task> GetWorkerTasks(int pageNumber, int pageSize, int workerId)
        {
            return
                GetContext().Task.Where(t => t.WorkerId == workerId).Skip(pageNumber * pageSize).Take(pageSize).ToArray();
        }

        public Task GetTask(int id)
        {
            return GetContext().Task.SingleOrDefault(t => t.Id == id);
        }

        public void SetReadiness(int percentage, int taskId)
        {
            Task task = GetTask(taskId);
            task.Percentage = percentage;
            GetContext().SaveChanges();
        }

        public void SaveManagerTask(int id, string name, string content, byte state, int workerId)
        {
            Task dbTask = GetContext().Task.Find(id);
            dbTask.Name = name;
            dbTask.Content = content;
            dbTask.State = state;

            if ((TaskState)state != TaskState.Open)
            {
                dbTask.WorkerId = workerId;
            }
            else
            {
                dbTask.WorkerId = null;
            }

            GetContext().SaveChanges();
        }

        public bool IsWorker(int workerId, int taskId)
        {
            Task task = GetTask(taskId);
            if ((TaskState)task.State == TaskState.Open || (TaskState)task.State == TaskState.Complete)
            {
                return false;
            }
            return task.WorkerId == workerId;
        }

        public User GetTaskWorker(Task task)
        {
            if (task.WorkerId == null)
            {
                return null;
            }
            return GetContext().User.SingleOrDefault(w => w.Id == task.WorkerId);
        }

        public User GetTaskWorker(int taskId)
        {
            Task task = GetTask(taskId);
            return GetTaskWorker(task);
        }

        public void SetPercentage(int id, int percentage)
        {
            Task task = GetTask(id);
            task.Percentage = percentage;
            GetContext().SaveChanges();
        }

        public void DeleteTask(int id)
        {
            Task task = GetContext().Task.Find(id);
            GetContext().Task.Remove(task);
            GetContext().SaveChanges();
        }
    }
}
