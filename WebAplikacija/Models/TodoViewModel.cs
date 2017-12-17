using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAplikacija.core;
namespace WebAplikacija.Models
{
    public class TodoViewModel : TodoItem
    {
       
       
       
        
        
       
        public TodoViewModel(Guid id, string text, DateTime? time, bool completed) : base(text)
        {
        }
        public TodoViewModel(string text, DateTime? time, bool completed) : this(Guid.NewGuid(), text, time, completed)
        {

        }

        public TodoViewModel(string text, DateTime? time) : this(text, time, false)
        {

        }
        public TodoViewModel()
        {
            
        }
    }
}
