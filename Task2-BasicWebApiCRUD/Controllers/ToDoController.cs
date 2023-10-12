using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Interfaces;
using Todo.Domain.Models;

namespace Task2_BasicWebApiCRUD.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ToDoModel model)
        {
            try
            {
                var addedItem = await _todoService.AddItem(model);
                return Ok(addedItem);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var items = await _todoService.GetAllItems();
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
                var item = await _todoService.GetItemById(id);
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
                var updatedItem = await _todoService.UpdateItem(id, model);
                if (updatedItem == null) return NotFound("Item not found");
                return Ok(updatedItem);
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
                var deleteResult = await _todoService.DeleteItem(id);
                return deleteResult ? Ok("Item removed") : NotFound("Item not found");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
