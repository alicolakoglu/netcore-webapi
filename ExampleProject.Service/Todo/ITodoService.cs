using System.Linq;
using System.Linq.Expressions;
using System;
using ExampleProject.Core.Domain.Todo;
using System.Collections.Generic;

namespace ExampleProject.Service
{
    public interface ITodoService
    {
        IQueryable<TodoItem> GetTodoItems();
        void InsertTodoItem(TodoItem entity);
        void UpdateTodoItem(TodoItem entity);
        void UpdateTodoItem(TodoItem entity, params Expression<Func<TodoItem, object>>[] properties);
        void DeleteTodoItem(TodoItem entity);

    }
}
