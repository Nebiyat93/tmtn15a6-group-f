using Microsoft.AspNetCore.Mvc.ModelBinding;
using Project.Models;
using System.Collections.Generic;

namespace Project.Interfaces
{
    public interface IValidateRecipe
    {
        bool ValidateProperties(Recipe properties, ModelStateDictionary modelState, string httpRequestMethod);
    }
}
