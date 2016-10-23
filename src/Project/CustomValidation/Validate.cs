using Microsoft.AspNetCore.Mvc.ModelBinding;
using Project.Interfaces;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.CustomValidation
{
    public class ValidateRecipe : IValidateRecipe
    {
        public bool ValidateProperties(Recipe properties, ModelStateDictionary modelState, string httpRequestMethod)
        {
            bool updateMethod;

            if (httpRequestMethod == "PATCH")
                updateMethod = true;
            else updateMethod = false;

            bool pass1 = ValidateName(properties.Name, modelState, updateMethod);
            bool pass2 = ValidateDescription(properties.Description, modelState, updateMethod);
            bool pass3 = ValidateDirections(properties.Directions, modelState, updateMethod);

            return pass1 && pass2 && pass3;
        }

        private bool ValidateName(string name, ModelStateDictionary modelState, bool updateMethod)
        {
            if (string.IsNullOrWhiteSpace(name) && !updateMethod)
                modelState.AddModelError("Name", "MissingName");

            else if (!string.IsNullOrWhiteSpace(name))
                if (name.Length < 5 || name.Length > 70) //Null-check performed above again due to logical operation here
                    modelState.AddModelError("Name", "NameWrongLength");

            return modelState.ErrorCount > 0 ? false : true;
        }

        private bool ValidateDescription(string description, ModelStateDictionary modelState, bool updateMethod)
        {
            if (string.IsNullOrWhiteSpace(description) && !updateMethod)
                modelState.AddModelError("Description", "MissingDescription");

            else if (!string.IsNullOrWhiteSpace(description))
                if (description.Length < 10 || description.Length > 300)
                    modelState.AddModelError("Description", "DescriptionWrongLength");

            return modelState.ErrorCount > 0 ? false : true;
        }

        private bool ValidateDirections(ICollection<Direction> directions, ModelStateDictionary modelState, bool updateMethod)
        {
            if (directions.Count < 1 && !updateMethod)
                modelState.AddModelError("Directions", "DirectionsMissing");
            else
            {
                foreach (var item in directions)
                {
                    if (item.Order == 0 && !updateMethod)
                        modelState.AddModelError("Order", "DirectionOrderMissing");

                    if (string.IsNullOrWhiteSpace(item.Description) && !updateMethod)
                        modelState.AddModelError("DirectionDescription", "DirectionDescriptionMissing");

                    else if (!string.IsNullOrWhiteSpace(item.Description))
                        if (item.Description.Length < 5 || item.Description.Length > 120)
                            modelState.AddModelError("DirectionDescription", "DirectionDescriptionWrongLength");
                }
            }
            return modelState.ErrorCount > 0 ? false : true;
        }
    }

    public class ValidateComment
    {
        public bool Validate(object item, ModelStateDictionary modelState, bool httpUpdate)
        {
            return true;
        }
    }
}
