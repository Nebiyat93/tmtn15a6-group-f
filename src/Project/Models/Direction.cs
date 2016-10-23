using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Project.Models
{
    public class Direction
    {
        public Direction() { }
        [Key]
        public int Id { get; set; }

        //[Range(1.0, double.MaxValue, ErrorMessage = "OrderMissing")]
        public int Order { get; set; }

        //[Required(ErrorMessage = "DescriptionMissing", AllowEmptyStrings = false)]
        //[StringLength(120, MinimumLength = 5, ErrorMessage = "DescriptionWrongLength")]
        public string Description { get; set; }
        
        public int RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; } 
    }
}
