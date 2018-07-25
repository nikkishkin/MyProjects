using System.Collections.Generic;
using System.Linq;
using TaskOperator.Entities;
using TaskOperator.Entities.Enums;

namespace TaskOperator.DAL.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(int id);

        User GetUser(string username);

        bool UserExists(string username);

        void AddUser(User user);

        bool IsFreeWorker(int id);

        IEnumerable<User> GetFreeWorkers();
    }
}
