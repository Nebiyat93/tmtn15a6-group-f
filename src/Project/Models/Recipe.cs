using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Recipe
    {
        public int Id;
        public char[] Name;
        public char[] Description;
        public char[] Image;
        public int Created;

        public Recipe()
        {
            Name = new char[70];
            Description = new char[300];
            Image = new char[200];
        }
    }
}
