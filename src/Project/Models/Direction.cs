using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Project.Models
{
    public class Direction
    {
        public Direction() { }
        public int Id { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        [MaxLength(120), MinLength(5)]
        public string Description { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } 

    }

    
}
