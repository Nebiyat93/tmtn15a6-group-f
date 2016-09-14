using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Project.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Comments = new HashSet<Comment>();
            Directions = new HashSet<Direction>();
            
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(70)][ MinLength(5)]
        public string Name { get; set; }
        [Required]
        [MaxLength(300)][ MinLength(10)]
        public string Description { get; set; }
        [MaxLength(200)][MinLength(0)]
        public string Image { get; set; }
        public int Created { get; set; }

        public string AccountId{get;set;}

        public ICollection<Comment> Comments { get; set; } 
        public ICollection<Direction> Directions { get; set; } 
        public ICollection<AccountRecipe> AccountRecipes { get; set; }

        public Account Account { get; set; } 
    }
}
