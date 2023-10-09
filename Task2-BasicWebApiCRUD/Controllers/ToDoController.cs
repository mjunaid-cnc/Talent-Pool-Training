using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2_BasicWebApiCRUD.Models;

namespace Task2_BasicWebApiCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        AppDbContext _context;

        public ToDoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ToDoModel model)
        {
            try
            {
                var todoItem = new ToDoModel
                {
                    Title = model.Title,
                };
                _context.ToDos.Add(todoItem);
                await _context.SaveChangesAsync();
                return Ok(todoItem);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var items = await _context.ToDos.OrderByDescending(x => x.UpdatedDate).ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            try
            {
                var item = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
                if (item == null) return NotFound("Item not found");
                return Ok(item);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromBody] ToDoModel model)
        {
            try
            {
                var itemToUpdate = await _context.ToDos.FirstOrDefaultAsync(x => x.Id == id);
                if (itemToUpdate == null) return NotFound("Item not found");
                itemToUpdate.Title = model.Title;
                itemToUpdate.IsCompleted = model.IsCompleted;
                itemToUpdate.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(itemToUpdate);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            try
            {
                var item = _context.ToDos.FirstOrDefault(x => x.Id == id);
                if (item == null) return NotFound("Item not found");
                _context.ToDos.Remove(item);
                await _context.SaveChangesAsync();
                return Ok("Item removed");
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
