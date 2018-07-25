using System.Collections.Generic;
using TaskOperator.Entities;

namespace TaskOperator.Logic.Interfaces
{
    public interface IUserBlo
    {
        User GetUser(int id);

        User GetUser(string username);

        bool UserExists(string username);

        void AddUser(User user);

        IEnumerable<User> GetFreeWorkers();

        bool IsFreeWorker(int id);
    }
}
