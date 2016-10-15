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
        private MyDbContext _context;

        public RecipeManager(MyDbContext context)
        {
            _context = context;
        }

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
            } while (_context.Recipes.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one
            recep.CreatorId = userId;
            recep.Created = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            recep.Directions.OrderBy(w => w.Order).ToList();
            _context.Recipes.Add(recep);

            _context.SaveChanges();
        }

        public Recipe Find(int id)
        {
            return (Recipe)_context.Recipes.Include(u => u.AccountIdentity).Include(u => u.Comments).Include(d => d.Directions).ToList().FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id)
        {
            var recep = _context.Recipes.FirstOrDefault(h => h.Id == id);
            _context.Recipes.Remove(recep);
            _context.SaveChanges();
        }

        public void Update(Recipe newRecipe, Recipe oldRecipe)
        {
            if (!string.IsNullOrWhiteSpace(newRecipe.Name))
                oldRecipe.Name = newRecipe.Name;
            if (!string.IsNullOrWhiteSpace(newRecipe.Description))
                oldRecipe.Description = newRecipe.Description;
            oldRecipe.Created = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (!string.IsNullOrWhiteSpace(newRecipe.Image))
                oldRecipe.Image = newRecipe.Image;

            _context.Update(oldRecipe);
            _context.SaveChanges();
        }
    }
}
