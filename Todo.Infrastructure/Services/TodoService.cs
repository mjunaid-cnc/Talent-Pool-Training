//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Todo.Application.Interfaces;
//using Todo.Domain.Models;

//namespace Todo.Infrastructure.Services
//{
//    public class TodoService : ITodoService
//    {
//        private readonly AppDbContext _context;
//        public TodoService(AppDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<ToDoModel> AddItem(ToDoModel item)
//        {
//            try
//            {
//                _context.TodoLists.Add(item);
//                await _context.SaveChangesAsync();
//                return item;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<List<ToDoModel>> GetAllItems()
//        {
//            try
//            {
//                return await _context.ToDos.OrderByDescending(x => x.UpdatedDate).ToListAsync();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<ToDoModel?> GetItemById(Guid id)
//        {
//            try
//            {
//                var item = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
//                return item;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<ToDoModel?> UpdateItem(Guid id, ToDoModel item)
//        {
//            try
//            {
//                var itemToUpdate = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
//                if (itemToUpdate == null) return null;
//                itemToUpdate.Title = item.Title;
//                itemToUpdate.IsCompleted = item.IsCompleted;
//                itemToUpdate.UpdatedDate = DateTime.Now;
//                await _context.SaveChangesAsync();
//                return itemToUpdate;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//        public async Task<bool> DeleteItem(Guid id)
//        {
//            try
//            {
//                var item = _context.TodoLists.FirstOrDefault(x => x.Id == id);
//                if (item == null) return false;
//                _context.TodoLists.Remove(item);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//    }
//}
