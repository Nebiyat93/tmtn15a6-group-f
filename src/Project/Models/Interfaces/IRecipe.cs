using System.Collections.Generic;


namespace Project.Models.Interfaces
{
    public interface IRecipe
    {
        Recipe Find(int Id);
        void Remove(int Id);
        void Update(Recipe oldRecipe, Recipe newRecipe);
        void Add(Recipe recp);
        IEnumerable<Recipe> GetAllSorted();
    }
}
