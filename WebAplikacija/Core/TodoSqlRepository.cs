using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAplikacija.Models;
namespace WebAplikacija.core
{
    class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }
        public async void AddAsync(TodoItem todoItem)
        {
            var test = await _context.Items.Include(m => m.Id).FirstAsync(s => s.Equals(todoItem));
            if (test.Id == todoItem.Id) throw new DuplicateTodoItemException("Already exists");
            else
            {
                _context.Items.Add(todoItem);
                _context.SaveChanges();
            }
        }

        public async Task<TodoItem> GetAsync(Guid todoId, Guid userId)
        {
            var test = await _context.Items.Include(s => s.Labels).Where(s => (s.Id == todoId)).FirstOrDefaultAsync();
            if (test == null) return null;
            else if (test.UserId != userId) throw new TodoAccessDeniedException("Youre not the owner");
            return test;

        }

        public async Task<List<TodoItem>> GetActiveAsync(Guid userId)
        {
            return await _context.Items.Where(s => s.UserId == userId).Where(s => s.DateCompleted == null).ToListAsync();
        }

        public async Task<List<TodoItem>> GetAllAsync(Guid userId)
        {
            return await _context.Items.Where(s => s.UserId == userId).OrderByDescending(s => s.DateCreated).ToListAsync();
        }

        public async Task<List<TodoItem>> GetCompletedAsync(Guid userId)
        {
            return await _context.Items.Where(s => s.UserId == userId).Where(s => s.DateCompleted != null).ToListAsync();
        }

        public async Task<List<TodoItem>> GetFilteredAsync(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return await _context.Items.Where(s => s.UserId == userId).Where(s => filterFunction(s) == true).ToListAsync();
        }

        public async Task<bool> MarkAsCompletedAsync(Guid todoId, Guid userId)
        {
            var test = await GetAsync(todoId, userId);
            if (test == null) return false;
            test.MarkAsCompleted();
            _context.SaveChanges();
            return true;

        }

        public async Task<bool> RemoveAsync(Guid todoId, Guid userId)
        {
            var test = await _context.Items.Include(s => s).FirstAsync(s => s.Id == todoId);
            if (test.Id == null) return false;
            if (todoId != userId) throw new TodoAccessDeniedException("Youre not the owner");
            _context.Items.Remove(test);
            _context.SaveChanges();
            return true;

        }

        public void UpdateAsync(TodoItem todoItem, Guid userId)
        {
            if(todoItem.UserId != userId) throw new TodoAccessDeniedException("Youre not the owner");
            AddAsync(todoItem);
        }
    }
}
