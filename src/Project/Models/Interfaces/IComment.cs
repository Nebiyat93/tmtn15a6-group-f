using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models.Interfaces
{
    public interface IComment
    {
        Comment Find(string Id);
        Comment Remove(string Id);
        void Update(Comment comm);
        void Add(Comment comm);
        IEnumerable<Comment> GetAll();
    }
}
