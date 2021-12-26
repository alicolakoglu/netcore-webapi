using ExampleProject.Core.Domain.Users;
using ExampleProject.Data.Repository;
using ExampleProject.Data.UnitOfWork;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace ExampleProject.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<User> _userRepository;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
            _userRepository = uow.GetRepository<User>();
        }
        public IQueryable<User> GetUsers()
        {
            return _userRepository.GetAll();
        }

        public void InsertUser(User entity)
        {
            _userRepository.Insert(entity);
        }

        public void UpdateUser(User entity)
        {
            _userRepository.Update(entity);
        }

        public void UpdateUser(User entity, params Expression<Func<User, object>>[] properties)
        {
            _userRepository.Update(entity, properties);
        }

        public void DeleteUser(User entity)
        {
            entity.IsDeleted = true;
            UpdateUser(entity, x => x.IsDeleted);
        }

        public void DeleteUser(Guid userKey)
        {
            var user = GetUsers().FirstOrDefault(x => x.Key == userKey);
            user.IsDeleted = true;
            UpdateUser(user, x => x.IsDeleted);
        }
    }
}
