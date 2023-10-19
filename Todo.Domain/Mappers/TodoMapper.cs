using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Entities;
using Todo.Domain.Models;

namespace Todo.Domain.Mappers
{
    public static class TodoMapper
    {
        public static TodoList AddTodoItem(ToDoModel model, Guid userId)
        {
            var newItem = new TodoList()
            {
                Title = model.Title,
                UserId = userId
            };
            return newItem;
        }

        public static TodoList UpdateTodoItem(ToDoModel model, TodoList itemToUpdate)
        {
            itemToUpdate.Title = model.Title;
            itemToUpdate.IsCompleted = model.IsCompleted;
            itemToUpdate.UpdatedDate = DateTime.Now;

            return itemToUpdate;
        }
    }
}
