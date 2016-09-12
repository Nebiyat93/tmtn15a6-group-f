using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Project.Models
{
    public class Comment
    {
        public Comment() { }

        public int Id { get; set; }
        [Required]
        [MaxLength(400), MinLength(10)]
        public string Text { get; set; }
        [Required]
        public int Grade { get; set; }
        [MaxLength(200), MinLength(0)]
        public string Image { get; set; }
        public int Created { get; set; }

        public int AccountId { get; set; }
        public int RecipeId { get; set; }

        public Account Account { get; set; } 
        public Recipe Recipe { get; set; } 
    }
}
