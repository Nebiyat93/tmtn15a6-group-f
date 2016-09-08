
using System.Collections.Generic;

namespace Project.Models.Interfaces
{
    public interface IComment
    {
        Comment Find(int Id);
        Comment Remove(int Id);
        void Update(Comment comm);
        void Add(Comment comm);
        IEnumerable<Comment> GetAll();
    }
}
