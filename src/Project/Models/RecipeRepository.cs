using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.Models;
using Project.SQL_Database;

namespace Project.Models
{
    public class RecipeRepository : IRecipe
    {
        private MyDbContext _context = new MyDbContext();

        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes;
        }

        public void Add(Recipe recep)
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode();
                if (id < 0)
                    id *= -1;
                recep.Id = id;

            } while (_context.Recipes.Any(h => h.Id == id));
            _context.Recipes.Add(recep);
            _context.SaveChanges();
        }

        public Recipe Find(int id)
        {
            return (Recipe)_context.Recipes.Where(h => h.Id == id).First();
        }

        public void Remove(int id)
        {
            var recep = _context.Recipes.Where(h => h.Id == id).First();
            _context.Remove(recep);
            _context.SaveChanges();
        }

        public void Update(Recipe recep)
        {
            var t = _context.Recipes.Where(h => h.Id == recep.Id).First();
            t.Name = recep.Name;
            t.Id = recep.Id;
            t.Image = recep.Image;
            t.Description = recep.Description;
            t.Created = recep.Created;
            t.AccountId = recep.AccountId;
            _context.SaveChanges();
            
        }
    }
}
