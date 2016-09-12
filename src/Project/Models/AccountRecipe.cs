using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class AccountRecipe
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
