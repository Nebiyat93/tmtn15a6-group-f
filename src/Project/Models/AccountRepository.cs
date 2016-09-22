using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using Project.SQL_Database;

namespace Project.Models
{
    public class AccountRepository : IAccount
    {
      
        private MyDbContext _context = new MyDbContext();

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts;
        }

        public void Add(Account acc)
        {
            acc.Id = Guid.NewGuid().ToString(); // generate new id
            _context.Accounts.Add(acc);
            _context.SaveChanges();
        }

        public Account Find(string id)
        {
            return _context.Accounts.First(p => p.Id == id);
        }

        public Account FindUser(string UserName)
        {
            return _context.Accounts.FirstOrDefault(p => p.UserName == UserName);
        }

        public void Remove(string Id)
        {
            _context.Remove(Find(Id));
            _context.SaveChanges();
        }

        public void Update(Account acc)
        {
            var _acc = _context.Accounts.Where(h => h.Id == acc.Id).First();
            _acc.UserName = acc.UserName;
            _acc.Id = acc.Id;
            _acc.Latitude = acc.Latitude;
            _acc.Longitude = acc.Longitude;
            _context.SaveChanges();
        }
    }
}
