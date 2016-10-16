using System.Collections.Generic;


namespace Project.Models.Interfaces
{
    public interface IRecipe
    {
        Recipe Find(int Id);
        void Remove(int Id);
        void Update(Recipe newRecipe, Recipe oldRecipe);
        void Add(Recipe recp, string userId);
        IEnumerable<Recipe> GetAll();
        IEnumerable<Recipe> GetAllSorted();
    }
}
