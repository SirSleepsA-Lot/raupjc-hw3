using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAplikacija.Models
{
    public class AddViewModel 
    {
        [Required]
        public string Text;
        [Required]
        public DateTime DateDue;
    }
}
