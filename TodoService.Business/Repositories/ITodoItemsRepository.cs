using System.Collections.Generic;
using System.Threading.Tasks;
using TodoService.Business.Models;

namespace TodoService.Business.Repositories
{
    public interface ITodoItemsRepository
    {
        public Task<IEnumerable<TodoItemDTO>> GetTodoItems();

        public Task<TodoItemDTO> GetTodoItem(long id);

        public Task<ResultStatus> UpdateTodoItem(long id, TodoItemDTO todoItemDTO);

        public Task<TodoItemDTO> CreateTodoItem(TodoItemDTO todoItemDTO);

        public Task<ResultStatus> DeleteTodoItem(long id);
    }
}
