using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoService.Business.Models;
using TodoService.Business.Repositories;
using TodoService.DataAccess.Context;

namespace TodoService.Business.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoItemsRepository _repository;
        private readonly TodoContext _context;

        public TodoItemsController(TodoItemsRepository repository, TodoContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var result = await _repository.GetTodoItems(_context);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _repository.GetTodoItem(id, _context);

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

            var resultStatus = await _repository.UpdateTodoItem(id, todoItemDTO, _context);

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
            return await _repository.CreateTodoItem(todoItemDTO, _context);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var resultStatus = await _repository.DeleteTodoItem(id, _context);

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
