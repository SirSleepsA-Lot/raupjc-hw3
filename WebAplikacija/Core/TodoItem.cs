using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAplikacija.core
{


    public class TodoItem : IEquatable<TodoItem>
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public DateTime? DateDue { get; set; }
        public string Text { get; set; }
        // Shorter syntax for single line getters in C#6
        // public bool IsCompleted = > DateCompleted . HasValue ;
        public bool IsCompleted
        {
            get
            {
                return DateCompleted.HasValue;
            }
        }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public TodoItem(string text)
        {
            // Generates new unique identifier
            Id = Guid.NewGuid();
            // DateTime .Now returns local time , it wont always be what you expect
            //(depending where the server is).
            // We want to use universal (UTC ) time which we can easily convert to
            //local when needed.
            // ( usually done in browser on the client side )
            DateCreated = DateTime.UtcNow;
            Text = text;
        }
        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }
        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }
        public TodoItem()
        {
            // entity framework needs this one
            // not for use :)
        }
        public bool Equals(TodoItem other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as TodoItem);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }



        public static bool operator ==(TodoItem a, TodoItem b)
        {
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }
        public static bool operator !=(TodoItem a, TodoItem b)
        {
            if (ReferenceEquals(a, b) && (ReferenceEquals(a, null) || ReferenceEquals(b, null))) return true;

            return !a.Equals(b);
        }
    }
    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException()
        {
        }

        public DuplicateTodoItemException(string message)
        : base(message)
        {
        }

        public DuplicateTodoItemException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException()
        {
        }
        public TodoAccessDeniedException(string message)
        : base(message)
        {
        }

        public TodoAccessDeniedException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
