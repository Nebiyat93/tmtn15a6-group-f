using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Project.Models
{
    public class AccountIdentity : IdentityUser
    {
        public AccountIdentity()
        {
            Comments = new HashSet<Comment>();
            Recipes = new HashSet<Recipe>();
        }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Recipe> Recipes { get; set; }   
        public ICollection<Favorites> Favorites { get; set; }
    }
}
