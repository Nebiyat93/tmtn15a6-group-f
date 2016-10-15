using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.CustomValidator
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Validator : ValidationAttribute
    { 
        //Inprogress
        public override bool IsValid(object value)
        {
            
            if (value == null)
            {
                return false;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
