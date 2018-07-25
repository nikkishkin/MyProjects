using System;
using System.Collections.Generic;
using TaskOperator.DAL.Interfaces;
using TaskOperator.Entities;
using TaskOperator.Logic.Interfaces;

namespace TaskOperator.Logic.Services
{
    public class TaskBlo: ITaskBlo
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmailService _emailService;

        public TaskBlo(ITaskRepository taskRepository, IEmailService emailService)
        {
            _taskRepository = taskRepository;
            _emailService = emailService;
        }

        public void CreateTask(string name)
        {
            Task task = new Task
            {
                Name = name,
                Date = DateTime.Now,
                Content = String.Empty
            };

            _taskRepository.AddTask(task);
        }

        public Task GetTask(int id)
        {
            return _taskRepository.GetTask(id);
        }

        public bool IsWorker(int workerId, int taskId)
        {
            return _taskRepository.IsWorker(workerId, taskId);
        }

        public void SaveManagerTask(int id, string name, string content, byte state, int workerId)
        {
            Task oldTask = GetTask(id);

            Task newTask = new Task
            {
                Id = id,
                Name = name,
                Content = content ?? string.Empty,
                State = state,
                WorkerId = workerId == -1 ? (int?)null : workerId
            };

            _emailService.CheckEmailSending(oldTask, newTask);

            _taskRepository.SaveManagerTask(id, name, content, state, workerId);
        }

        public void SetPercentage(int id, int percentage)
        {
            Task oldTask = GetTask(id);

            _emailService.CheckEmailSending(oldTask, percentage);

            _taskRepository.SetPercentage(id, percentage);
        }

        //public bool ManagerPageExists(int pageNumber, int pageSize)
        //{
        //    return _taskRepository.ManagerPageExists(pageNumber, pageSize);
        //}

        public IEnumerable<Task> GetManagerTasks(int pageNumber, int pageSize)
        {
            return _taskRepository.GetManagerTasks(pageNumber, pageSize);
        }

        //public bool WorkerPageExists(int pageNumber, int pageSize, int workerId)
        //{
        //    return _taskRepository.WorkerPageExists(pageNumber, pageSize, workerId);
        //}

        public IEnumerable<Task> GetWorkerTasks(int pageNumber, int pageSize, int workerId)
        {
            return _taskRepository.GetWorkerTasks(pageNumber, pageSize, workerId);
        }

        public int GetAllTasksCount()
        {
            return _taskRepository.GetAllTasksCount();
        }

        public int GetWorkerTasksCount(int workerId)
        {
            return _taskRepository.GetWorkerTasksCount(workerId);
        }

        public User GetTaskWorker(Task task)
        {
            return _taskRepository.GetTaskWorker(task);
        }

        public User GetTaskWorker(int taskId)
        {
            return _taskRepository.GetTaskWorker(taskId);
        }

        public void DeleteTask(int id)
        {
            _taskRepository.DeleteTask(id);
        }
    }
}
