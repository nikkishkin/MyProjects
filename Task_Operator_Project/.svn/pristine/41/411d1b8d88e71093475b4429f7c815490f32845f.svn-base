using System.Collections.Generic;
using TaskOperator.Core;
using TaskOperator.DAL.Interfaces;
using TaskOperator.DAL.Repository;
using TaskOperator.Entities;
using TaskOperator.Logic.Interfaces;

namespace TaskOperator.Logic.Services
{
    public class UserBlo : IUserBlo
    {
        private readonly IUserRepository _userRepository;

        public UserBlo(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUser(int id)
        {
            return _userRepository.GetUser(id);
        }

        public User GetUser(string username)
        {
            return _userRepository.GetUser(username.Trim());
        }

        public bool UserExists(string username)
        {
            return _userRepository.UserExists(username.Trim());
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public IEnumerable<User> GetFreeWorkers()
        {
            return _userRepository.GetFreeWorkers();
        }

        public bool IsFreeWorker(int id)
        {
            return _userRepository.IsFreeWorker(id);
        }
    }
}
