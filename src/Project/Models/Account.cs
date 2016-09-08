using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string UserName;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
