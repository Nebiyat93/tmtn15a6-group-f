using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Comment
    {
        public int Id;
        public char[] Text;
        public int Grade;
        public char[] Image;
        public int Created;

        public Comment()
        {
            Text = new char[400];
            Image = new char[200];
        }
    }
}
