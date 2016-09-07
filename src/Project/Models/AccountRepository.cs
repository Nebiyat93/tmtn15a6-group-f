using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;

namespace Project.Models
{
    public class AccountRepository : IAccount
    {
        private static ConcurrentDictionary<string, Account> _accs =
            new ConcurrentDictionary<string, Account>();

        public AccountRepository()
        {
            Add(new Account { Id = Guid.NewGuid().ToString() });
        }

        public IEnumerable<Account> GetAll()
        {
            return _accs.Values;
        }

        public void Add(Account acc)
        {
            acc.Id = Guid.NewGuid().ToString();
            _accs[acc.Id] = acc;
        }

        public Account Find(string id)
        {
            Account acc;
            _accs.TryGetValue(id, out acc);
            return acc;
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
        }
    }
}
