using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TodoService.Business.Models;
using TodoService.Business.Repositories;
using TodoService.Business.Tests.FakeContext;
using TodoService.DataAccess.Context;

namespace TodoService.Business.Tests
{
    public class TodoServiceRepositoryTests : IDisposable
    {
        private TodoItemsRepository repository;
        private TodoContext context;

        private TodoItemDTO fakeItem;

        public TodoServiceRepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<TodoContext>();
            builder.UseInMemoryDatabase("TodoList");
            var options = builder.Options;
            context = new FakeTodoContext(options);
            repository = new TodoItemsRepository(context);

            fakeItem = new TodoItemDTO { Name = "FakeItem1", IsComplete = false };
        }

        [Fact]
        public void GetTodoItems_ReturnNotNull()
        {
            var result = repository.GetTodoItems();

            Assert.NotNull(result.Result);
        }

        [Theory]
        [InlineData("Alpha")]
        [InlineData("Beta")]
        public void GetTodoItem_ReturnTheItem(string itemName)
        {
            fakeItem.Name = itemName;
            var resultCreated = repository.CreateTodoItem(fakeItem);
            var itemId = resultCreated?.Result?.Id ?? 0L;

            var result = repository.GetTodoItem(itemId);

            Assert.NotNull(result.Result);
            Assert.Equal(result.Result.Id, itemId);
            Assert.Equal(result.Result.Name, itemName);
        }

        [Theory]
        [InlineData("Beta")]
        [InlineData("Gamma")]
        public void UpdateTodoItem_ReturnNoContent(string itemName)
        {
            var resultCreated = repository.CreateTodoItem(fakeItem);
            var itemId = resultCreated?.Result?.Id ?? 0L;

            fakeItem.Id = itemId;
            fakeItem.Name = itemName;

            var updateStatus = repository.UpdateTodoItem(itemId, fakeItem);

            var result = repository.GetTodoItem(itemId);

            Assert.Equal((short)updateStatus.Result, (short)ResultStatus.NoContent);
            Assert.NotNull(result.Result);
            Assert.Equal(result.Result.Name, itemName);
        }

        [Theory]
        [InlineData("Delta", false)]
        public void CreateTodoItem_ReturnTheSameItem(string itemName, bool isComplete)
        {
            fakeItem.Name = itemName;
            fakeItem.IsComplete = isComplete;

            var resultCreated = repository.CreateTodoItem(fakeItem);
            var itemId = resultCreated?.Result?.Id ?? 0L;

            var result = repository.GetTodoItem(itemId);

            Assert.NotNull(result.Result);
            Assert.Equal(result.Result.Name, itemName);
            Assert.Equal(result.Result.IsComplete, isComplete);
        }

        [Fact]
        public void DeleteTodoItem_ReturnNoContent()
        {
            var resultCreated = repository.CreateTodoItem(fakeItem);
            var itemId = resultCreated?.Result?.Id ?? 0L;

            var result = repository.DeleteTodoItem(itemId);

            Assert.Equal((short)result.Result, (short)ResultStatus.NoContent);
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }
}
