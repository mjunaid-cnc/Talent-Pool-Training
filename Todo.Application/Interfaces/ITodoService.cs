using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Models;

namespace Todo.Application.Interfaces
{
    public interface ITodoService
    {
        Task<ToDoModel> AddItem(ToDoModel item);
        Task<List<ToDoModel>> GetAllItems();
        Task<ToDoModel?> GetItemById(Guid id);
        Task<ToDoModel?> UpdateItem(Guid id, ToDoModel item);
        Task<bool> DeleteItem(Guid id);
    }
}
