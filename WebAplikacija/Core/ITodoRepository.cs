using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAplikacija.Models;
namespace WebAplikacija.core
{
    public interface ITodoRepository
    {
        Task<TodoItem> GetAsync(Guid todoId, Guid userId);
        void AddAsync(TodoItem todoItem);
        Task<bool> RemoveAsync(Guid todoId, Guid userId);
        void UpdateAsync(TodoItem todoItem, Guid userId);
        Task<bool> MarkAsCompletedAsync(Guid todoId, Guid userId);
        Task<List<TodoItem>> GetAllAsync(Guid userId);
        Task<List<TodoItem>> GetActiveAsync(Guid userId);
        Task<List<TodoItem>> GetCompletedAsync(Guid userId);
        Task<List<TodoItem>> GetFilteredAsync(Func<TodoItem, bool> filterFunction, Guid userId);
    }
}
