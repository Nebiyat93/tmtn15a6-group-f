using System.Collections.Generic;

namespace Project.Models.Interfaces
{
    public interface IComment
    {
        Comment Find(int Id);
        void Remove(int Id);
        void Update(Comment comm);
        void Add(Comment comm, string commenterId);
        IEnumerable<Comment> GetAll();
    }
}
