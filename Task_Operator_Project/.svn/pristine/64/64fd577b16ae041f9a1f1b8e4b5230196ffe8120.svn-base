using System;
using System.Collections.Generic;
using System.Linq;
using TaskOperator.Core;
using TaskOperator.DAL.Interfaces;
using TaskOperator.DAL.UnitOfWork;
using TaskOperator.Entities;
using TaskOperator.Entities.Enums;

namespace TaskOperator.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataProvider _dataProvider;

        public UserRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        private TaskOperatorEntities GetContext()
        {
            return ((IEntityFrameworkUnitOfWork)(_dataProvider.GetUnitOfWork())).GetContext();
        }

        public User GetUser(int id)
        {
            return GetContext().User.SingleOrDefault(u => u.Id == id);
        }

        public User GetUser(string username)
        {
            return GetContext().User.SingleOrDefault(u => u.Username == username);
        }

        public bool UserExists(string username)
        {
            return GetContext().User.Any(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            GetContext().User.Add(user);
            GetContext().SaveChanges();
        }

        public bool IsFreeWorker(int id)
        {
            //return _unitOfWork.GetContext().Task.All(t => t.WorkerId != user.Id) && !user.IsManager;
            User user = GetContext().User.Find(id);
            return user.Task.Count == 0 || user.Task.All(t => (TaskState) (t.State) == TaskState.Complete);
        }

        public IEnumerable<User> GetFreeWorkers()
        {
            return GetContext().User
                .Where(u => !u.IsManager && 
                    (u.Task.Count == 0 || u.Task.All(t => (TaskState) (t.State) == TaskState.Complete)))
                .ToArray();
        }
    }
}
