using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Account
    {
        public Account()
        {
            Comments = new HashSet<Comment>();
            Recipes = new HashSet<Recipe>();
        }
        
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(12)][MinLength(1)]
        public string UserName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Comment> Comments { get; set; } 
        public ICollection<Recipe> Recipes { get; set; } 

        public ICollection<AccountRecipe> AccountRecipes {get; set;}

    }
}
