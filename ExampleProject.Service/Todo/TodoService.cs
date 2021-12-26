using ExampleProject.Data.Repository;
using ExampleProject.Data.UnitOfWork;
using System;
using System.Linq;
using System.Linq.Expressions;
using ExampleProject.Core.Domain.Todo;

namespace ExampleProject.Service
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<TodoItem> _todoItemRepository;

        public TodoService(IUnitOfWork uow)
        {
            _uow = uow;
            _todoItemRepository = uow.GetRepository<TodoItem>();
        }

        public IQueryable<TodoItem> GetTodoItems()
        {
            return _todoItemRepository.GetAll();
        }
        public void InsertTodoItem(TodoItem entity)
        {
            _todoItemRepository.Insert(entity);
        }
        public void UpdateTodoItem(TodoItem entity)
        {
            _todoItemRepository.Update(entity);
        }
        public void UpdateTodoItem(TodoItem entity, params Expression<Func<TodoItem, object>>[] properties)
        {
            _todoItemRepository.Update(entity, properties);
        }
        public void DeleteTodoItem(TodoItem entity)
        {
            _todoItemRepository.Delete(entity);
        }

    }
}
