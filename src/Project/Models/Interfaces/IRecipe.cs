using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Interfaces
{
    public interface IRecipe
    {
        Recipe Find(string Id);
        Recipe Remove(string Id);
        void Update(Recipe recp);
        void Add(Recipe recp);
        IEnumerable<Recipe> GetAll();
    }
}
