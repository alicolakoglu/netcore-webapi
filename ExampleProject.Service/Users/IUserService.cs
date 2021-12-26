using System.Linq;
using System.Linq.Expressions;
using System;
using ExampleProject.Core.Domain.Users;

namespace ExampleProject.Service
{
    public interface IUserService
    {
        IQueryable<User> GetUsers();
        void InsertUser(User entity);
        void UpdateUser(User entity);
        void UpdateUser(User entity, params Expression<Func<User, object>>[] properties);
        void DeleteUser(User entity);
        void DeleteUser(Guid userKey);
    }
}
