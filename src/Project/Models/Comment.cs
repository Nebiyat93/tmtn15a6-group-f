using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public char[] Text;
        public int Grade { get; set; }
        public char[] Image;
        public int Created { get; set; }
    }
}
