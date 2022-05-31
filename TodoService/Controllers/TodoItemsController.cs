using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoService.Business.Models;
using TodoService.Business.Repositories;

namespace TodoService.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoItemsRepository _repository;

        public TodoItemsController(TodoItemsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var result = await _repository.GetTodoItems();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _repository.GetTodoItem(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var resultStatus = await _repository.UpdateTodoItem(id, todoItemDTO);

            switch (resultStatus)
            {
                case ResultStatus.NotFound:
                    return NotFound();
                case ResultStatus.NoContent:
                    return NoContent();
                default:
                    return Ok();
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            return await _repository.CreateTodoItem(todoItemDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var resultStatus = await _repository.DeleteTodoItem(id);

            switch (resultStatus)
            {
                case ResultStatus.NotFound:
                    return NotFound();
                case ResultStatus.NoContent:
                    return NoContent();
                default:
                    return Ok();
            }
        }
    }
}
