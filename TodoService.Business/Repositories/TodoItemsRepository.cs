using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoService.Business.Models;
using TodoService.DataAccess.Context;
using TodoService.DataAccess.Models;

namespace TodoService.Business.Repositories
{
    public class TodoItemsRepository
    {
        public async Task<IEnumerable<TodoItemDTO>> GetTodoItems(TodoContext context)
        {
            return await context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        public async Task<TodoItemDTO> GetTodoItem(long id, TodoContext context)
        {
            var todoItem = await context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        public async Task<ResultStatus> UpdateTodoItem(long id, TodoItemDTO todoItemDTO, TodoContext context)
        {
            if (id != todoItemDTO.Id)
            {
                return ResultStatus.BadRequest;
            }

            var todoItem = await context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return ResultStatus.NotFound;
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id, context))
            {
                return ResultStatus.NotFound;
            }

            return ResultStatus.NoContent;
        }

        public async Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO, TodoContext context)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();

            return ItemToDTO(todoItem);
        }

        public async Task<ResultStatus> DeleteTodoItem(long id, TodoContext context)
        {
            var todoItem = await context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return ResultStatus.NotFound;
            }

            context.TodoItems.Remove(todoItem);
            await context.SaveChangesAsync();

            return ResultStatus.NoContent;
        }

        private bool TodoItemExists(long id, TodoContext context) =>
             context.TodoItems.Any(e => e.Id == id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
