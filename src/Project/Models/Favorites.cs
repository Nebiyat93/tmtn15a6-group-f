using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Favorites
    {
        public string AccountId { get; set; }
        public AccountIdentity AccountIdentity { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
