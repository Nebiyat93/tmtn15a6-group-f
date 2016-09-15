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
        private static ConcurrentDictionary<string, Account> _accs =
            new ConcurrentDictionary<string, Account>();

        private MyDbContext _context = new MyDbContext();

        public IEnumerable<Account> GetAll()
        {
            var acc = _context.Accounts;

            return acc;
        }

        public void Add(Account acc)
        {
            acc.Id = Guid.NewGuid().ToString(); // generate new id
            //_accs[acc.Id] = acc;
            _context.Accounts.Add(acc);
            _context.SaveChanges();
        }

        public Account Find(string id)
        {
            
            var _acc = _context.Accounts.First(p => p.Id == id);
            return _acc;
          
        }

        public Account FindUser(string UserName)
        {

            var _acc = _context.Accounts.FirstOrDefault(p => p.UserName == UserName);
            return _acc;

        }

        public Account Remove(string Id)
        {
            Account acc;
            _accs.TryRemove(Id, out acc);
            return acc;
        }

        public void Update(Account acc)
        {
            _accs[acc.Id] = acc;
            _context.SaveChanges();
        }
    }
}
