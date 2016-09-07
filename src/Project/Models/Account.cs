using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Account
    {
        public string Id { get; set; }
        public char[] UserName;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Account()
        {
            UserName = new char[] { 'c' };
        }
    }
}
