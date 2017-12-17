using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAplikacija.Models
{
    public class IndexViewModel : IEnumerable<TodoViewModel>
    {
        public List<TodoViewModel> List { get; }

        public IndexViewModel()
        {
            List = new List<TodoViewModel>();
        }

        public void Add(TodoViewModel model)
        {
            List.Add(model);
        }

        public IEnumerator<TodoViewModel> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        
    }
}
