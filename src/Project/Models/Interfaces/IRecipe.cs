using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Interfaces
{
    public interface IRecipe
    {
        Recipe Find(int Id);
        Recipe Remove(int Id);
        void Update(Recipe recp);
        void Add(Recipe recp);
        IEnumerable<Recipe> GetAll();
    }
}
