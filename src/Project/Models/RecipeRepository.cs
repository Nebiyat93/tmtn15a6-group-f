using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.Models;

namespace Project.Models
{
    public class RecipeRepository : IRecipe
    {
        private static ConcurrentDictionary<int, Recipe> _recep =
            new ConcurrentDictionary<int, Recipe>();

        public RecipeRepository()
        {
            Add(new Recipe { Id = 0 });
        }

        public IEnumerable<Recipe> GetAll()
        {
            return _recep.Values;
        }

        public void Add(Recipe recep)
        {
            recep.Id = 0;
            _recep[recep.Id] = recep;
        }

        public Recipe Find(int id)
        {
            Recipe recep;
            _recep.TryGetValue(id, out recep);
            return recep;
        }

        public Recipe Remove(int id)
        {
            Recipe recep;
            _recep.TryRemove(id, out recep);
            return recep;
        }

        public void Update(Recipe recep)
        {
            _recep[recep.Id] = recep;
        }
    }
}
