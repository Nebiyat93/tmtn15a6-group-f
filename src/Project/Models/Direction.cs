using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Direction
    {
        public int Id;
        public int Order;
        public char[] Description;

        public Direction()
        {
            Description = new char[120];
        }
    }
}
