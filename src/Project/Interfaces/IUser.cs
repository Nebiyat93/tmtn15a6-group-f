using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Models.Interfaces
{
    public interface IUser
    {
        bool ValidateUser(string userId);
        AccountIdentity getCurrentUser(string userId);
    }
}
