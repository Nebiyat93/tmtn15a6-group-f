using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.Models;
using Project.SQL_Database;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class RecipeManager : IRecipe
    {
        private MyDbContext _context = new MyDbContext();

        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes;
        }

        public IEnumerable<Recipe> GetAllSorted()
        {
            return _context.Recipes.Include(u => u.Comments).Include(d => d.Directions).OrderByDescending(r => r.Created);
        }

        public void Add(Recipe recep, string userId)
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode(); //Returns numbers from GUID
                if (id < 0)
                    id *= -1;
                recep.Id = id;
                recep.CreatorId = userId;
                recep.Created = generateUnixTimestamp();
            } while (_context.Recipes.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one
            _context.Recipes.Add(recep);


            ///No logic for account existance yet!!!!
            //var user = _context.Users.First(p => p.Id == recep.CreatorId);
            //user.Recipes.Add(recep);
            //_context.Users.Update(user);
            _context.SaveChanges();
        }

        public Recipe Find(int id)
        {
            return (Recipe)_context.Recipes.Include(u => u.AccountIdentity).Include(u => u.Comments).Include(d => d.Directions).ToList().FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id)
        {
            var recep = _context.Recipes.Where(h => h.Id == id).First();
            _context.Remove(recep);
            _context.SaveChanges();
        }

        public void Update(Recipe newRecipe, Recipe oldRecipe)
        {
            if (!string.IsNullOrWhiteSpace(newRecipe.Name))
                oldRecipe.Name = newRecipe.Name;
            if (!string.IsNullOrWhiteSpace(newRecipe.Description))
                oldRecipe.Description = newRecipe.Description;
            oldRecipe.Created = generateUnixTimestamp();

            _context.SaveChanges();
        }


        /// <summary>
        /// Gives number of seconds between current time and 1970/01/01
        /// </summary>
        /// <returns></returns>
        public static Int32 generateUnixTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
