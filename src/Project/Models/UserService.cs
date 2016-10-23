using Project.Models.Interfaces;
using Project.SQL_Database;
using System;
using System.Linq;

namespace Project.Models
{
    public class UserService : IUser
    {
        private readonly MyDbContext _context;
        public UserService(MyDbContext context)
        {
            _context = context;
        }

        public bool ValidateUser(string userId)
        {
            if (_context.Users.Any(w => w.Id == userId))
                return true;
            else return false;
        }

        public AccountIdentity getCurrentUser(string userId)
        {
            return _context.Users.FirstOrDefault(w => w.Id == userId);
        }
    }
}
