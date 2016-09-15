
using System.Collections.Generic;

namespace Project.Models.Interfaces
{
    public interface IAccount
    {
        void Add(Account acc);
        IEnumerable<Account> GetAll();
        Account Find(string Id);
        Account FindUser(string UserName);
        Account Remove(string Id);
        void Update(Account acc);
    }
}
