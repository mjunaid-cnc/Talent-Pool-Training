using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;
using Todo.Domain.Helpers;
using Todo.Domain.Mappers;
using Todo.Domain.Models;

namespace Todo.Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserResolverService _userResolverService;
        public TodoService(AppDbContext context, IHttpContextAccessor httpContext, IUserResolverService userResolverService)
        {
            _context = context;
            _httpContext = httpContext;
            _userResolverService = userResolverService;
        }
        public async Task<TodoList> AddItem(ToDoModel item)
        {
            try
            {
                var userIdClaim = _httpContext.HttpContext!.User.FindFirstValue("id");
                bool parseSuccess = Guid.TryParse(userIdClaim, out Guid userId);
                if (!parseSuccess) throw new ApplicationException("Invalid user id");

                var toDoItem = TodoMapper.AddTodoItem(item, userId);
                _context.TodoLists.Add(toDoItem);
                await _context.SaveChangesAsync();
                return toDoItem;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TodoList?> GetItemById(Guid id)
        {
            try
            {
                var role = _userResolverService.GetUserRole();
                var userId = _userResolverService.GetUserId();
                TodoList? item;
                if (string.Equals(role, UserRoleType.User.ToString()))
                {
                    item = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
                }
                else
                {
                    item = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
                }
                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TodoList?> UpdateItem(Guid id, ToDoModel item)
        {
            try
            {
                var userRole = _userResolverService.GetUserRole();
                var userId = _userResolverService.GetUserId();
                var allLists = _context.TodoLists.ToList();
                TodoList? itemToUpdate;
                if (string.Equals(userRole, UserRoleType.User.ToString()))
                {
                    itemToUpdate = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
                }
                else
                {
                    itemToUpdate = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
                }
                if (itemToUpdate == null) return null;
                var updatedItem = TodoMapper.UpdateTodoItem(item, itemToUpdate);
                await _context.SaveChangesAsync();
                return updatedItem;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteItem(Guid id)
        {
            try
            {
                var userRole = _userResolverService.GetUserRole();
                var userId = _userResolverService.GetUserId();
                var allLists = _context.TodoLists.ToList();
                
                TodoList? itemToDelete;
                if (string.Equals(userRole, UserRoleType.User.ToString()))
                {
                    itemToDelete = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
                }
                else
                {
                    itemToDelete = await _context.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
                }
                if (itemToDelete == null) return false;
                _context.TodoLists.Remove(itemToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TodoList>> GetTodoList()
        {
            try
            {
                var userRole = _userResolverService.GetUserRole();
                var userId = _userResolverService.GetUserId();
                var allTodoLists = await _context.TodoLists.ToListAsync();
                if (Equals(userRole, UserRoleType.User.ToString()))
                {
                   allTodoLists = allTodoLists.Where(x => x.UserId == userId).ToList();
                }
                return allTodoLists;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
