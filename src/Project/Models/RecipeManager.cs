﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.Models;
using Project.SQL_Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Project.CustomValidation;

namespace Project.Models
{
    public class RecipeManager : IRecipe
    {
        private readonly MyDbContext _context;
        private DirectionManager _directionManager;

        public RecipeManager(MyDbContext context)
        {
            _context = context;
            _directionManager = new DirectionManager(context);
        }

        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes;
        }

        public IEnumerable<Recipe> GetAllSorted()
        {
            return _context.Recipes.Include(u => u.Comments).Include(d => d.Directions).OrderByDescending(r => r.Created);
        }

        public void Add(Recipe recep, AccountIdentity user)
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode(); //Returns numbers from GUID
                if (id < 0)
                    id *= -1;
                recep.Id = id;
            } while (_context.Recipes.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one
            recep.CreatorId = user.Id;
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
            
            _context.Recipes.Remove(Find(id));
            _context.SaveChanges();
        }

        public void Update(Recipe newRecipe, Recipe oldRecipe)
        {
            //if (!ValidateRecipe(newRecipe, null, false))
            //    return;

            if (!string.IsNullOrWhiteSpace(newRecipe.Name))
                oldRecipe.Name = newRecipe.Name;
            if (!string.IsNullOrWhiteSpace(newRecipe.Description))
                oldRecipe.Description = newRecipe.Description;
            if (newRecipe.Directions.Any(w => w.Order != 0))
                oldRecipe.Directions.Where(w => w.Order != 0).ToList().ForEach(h => h.Order = newRecipe.Directions.Select(p => p.Order).FirstOrDefault());
            if (!string.IsNullOrWhiteSpace(newRecipe.Directions.Select(w => w.Description).FirstOrDefault()))
                oldRecipe.Directions.Where(w => !string.IsNullOrWhiteSpace(w.Description)).ToList().ForEach(h => h.Description = newRecipe.Directions.Select(p => p.Description).FirstOrDefault());
            oldRecipe.Directions = newRecipe.Directions.OrderBy(w => w.Order).ToList();

            if (!string.IsNullOrWhiteSpace(newRecipe.Image))
                oldRecipe.Image = newRecipe.Image;

            oldRecipe.Created = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            _context.Recipes.Update(oldRecipe);
            _context.SaveChanges();
        }
    }
}
