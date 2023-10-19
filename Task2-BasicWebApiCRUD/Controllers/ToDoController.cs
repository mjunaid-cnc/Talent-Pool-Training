using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2_BasicWebApiCRUD.ActionFilters;
using Todo.Application.Interfaces;
using Todo.Domain.Entities;
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
                if (!ModelState.IsValid)
                {
                    return Ok(new Response
                    {
                        Message = "Validation failed",
                        Content = ModelState.Values.Select(x => x.Errors),
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }
                var addedItem = await _todoService.AddItem(model);
                return Ok(new Response { Content = addedItem, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodoLists()
        {
            try
            {
                var todoList = await _todoService.GetTodoList();
                return Ok(new Response { Content = todoList, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [ServiceFilter(typeof(CheckUserAccessActionFilter))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            try
            {
                var item = await _todoService.GetItemById(id);
                if (item == null) return Ok(new Response { Message = "Item not found", StatusCode = StatusCodes.Status400BadRequest });
                return Ok(new Response { Content = item, StatusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }


        [ServiceFilter(typeof(CheckUserAccessActionFilter))]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromBody] ToDoModel model)
        {
            try
            {
                var updatedItem = await _todoService.UpdateItem(id, model);
                if (updatedItem == null) return Ok(new Response { Message = "Item not found", StatusCode = StatusCodes.Status400BadRequest });
                return Ok(new Response { Content = updatedItem, StatusCode = StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [ServiceFilter(typeof(CheckUserAccessActionFilter))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            try
            {
                var deleteResult = await _todoService.DeleteItem(id);
                return deleteResult ? Ok(new Response { Message = "Item removed", StatusCode = StatusCodes.Status200OK }) : Ok(new Response { Message = "Item not found", StatusCode = StatusCodes.Status400BadRequest });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

      
    }
}
