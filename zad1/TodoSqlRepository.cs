using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad1
{
    class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }
        public void Add(TodoItem todoItem)
        {
            var test = _context.Items.Include(m => m.Id).First(s => s.Equals(todoItem));
            if (test.Id == todoItem.Id) throw new DuplicateTodoItemException("Already exists");
            else
            {
                _context.Items.Add(todoItem);
                _context.SaveChanges();
            }
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var test = _context.Items.Include(s => s).First(s => s.Id == todoId);
            if (test.Id == null) return null;
            if (todoId != userId) throw new TodoAccessDeniedException("Youre not the owner");
            return test;
            
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.Items.Include(s => s).Where(s => s.UserId == userId).Where(s => s.DateCompleted == null).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.Items.Include(s => s).Where(s => s.UserId == userId).OrderByDescending(s => s.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.Items.Include(s => s).Where(s => s.UserId == userId).Where(s => s.DateCompleted != null).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.Items.Include(s => s).Where(s => s.UserId == userId).Where(s => filterFunction(s) == true).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var test = Get(todoId, userId);
            if (test == null) return false;
            test.MarkAsCompleted();
            _context.SaveChanges();
            return true;

        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var test = _context.Items.Include(s => s).First(s => s.Id == todoId);
            if (test.Id == null) return false;
            if(todoId != userId) throw new TodoAccessDeniedException("Youre not the owner");
            _context.Items.Remove(test);
            _context.SaveChanges();
            return true;
            
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem.UserId != userId) throw new TodoAccessDeniedException("Youre not the owner");
            Add(todoItem);
        }
    }
}
