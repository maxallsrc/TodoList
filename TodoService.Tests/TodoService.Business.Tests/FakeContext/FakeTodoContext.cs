using Microsoft.EntityFrameworkCore;
using TodoService.DataAccess.Context;

namespace TodoService.Business.Tests.FakeContext
{
    public class FakeTodoContext : TodoContext
    {
        public FakeTodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }
    }
}
