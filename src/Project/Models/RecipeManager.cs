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
        private readonly MyDbContext _context;
        public IUpload imageHelp { get; set; }
        public RecipeManager(MyDbContext context, IUpload imageHelp)
        {
            _context = context;
            this.imageHelp = imageHelp;
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
            var user = _context.Users.First(p => p.Id == userId);
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
            recep.AccountIdentity = user;
            recep.Directions.OrderBy(w => w.Order).ToList();
            _context.Recipes.Add(recep);
   
            user.Recipes.Add(recep);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public Recipe Find(int id)
        {
            return (Recipe)_context.Recipes.Include(u => u.AccountIdentity).Include(u => u.Comments).Include(d => d.Directions).ToList().FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id)
        {
            var parent = Find(id);
            foreach(var comm in parent.Comments)
            {
                if (comm.Image != null)
                {
                    var im = new Uri(comm.Image);
                    var name = im.Segments[im.Segments.Length - 1];
                    var test = imageHelp.Remove(name);
                }

                _context.Remove(comm);
            }
            //_context.Comments.RemoveRange(parent.Comments);
            _context.Remove(parent);
            _context.SaveChanges();
        }



        public void Update(Recipe newRecipe, Recipe oldRecipe)
        {

            if (!string.IsNullOrWhiteSpace(newRecipe.Name))
                oldRecipe.Name = newRecipe.Name;
            if (!string.IsNullOrWhiteSpace(newRecipe.Description))
                oldRecipe.Description = newRecipe.Description;
            if (newRecipe.Directions != null)
                oldRecipe.Directions = newRecipe.Directions.OrderBy(w => w.Id).ToList();

            if (!string.IsNullOrWhiteSpace(newRecipe.Image))
                oldRecipe.Image = newRecipe.Image;

            oldRecipe.Created = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            _context.Recipes.Update(oldRecipe);
            _context.SaveChanges();
        }
    }
}
