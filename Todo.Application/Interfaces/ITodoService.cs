using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Entities;
using Todo.Domain.Models;

namespace Todo.Application.Interfaces
{
    public interface ITodoService
    {
        Task<TodoList> AddItem(ToDoModel item);
        Task<TodoList?> GetItemById(Guid id);
        Task<TodoList?> UpdateItem(Guid id, ToDoModel item);
        Task<bool> DeleteItem(Guid id);
        Task<List<TodoList>> GetTodoList();
    }
}
