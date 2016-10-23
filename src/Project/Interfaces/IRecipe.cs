using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;


namespace Project.Models.Interfaces
{
    public interface IRecipe
    {
        Recipe Find(int Id);
        void Remove(int Id);
        void Update(Recipe newRecipe, Recipe oldRecipe);
        void Add(Recipe recp, AccountIdentity user);
        //bool ValidateRecipe(Recipe recep, ModelStateDictionary modelstate, bool UpdateRecipe);
        IEnumerable<Recipe> GetAll();
        IEnumerable<Recipe> GetAllSorted();
    }
}
