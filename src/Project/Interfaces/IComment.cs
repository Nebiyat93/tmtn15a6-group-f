using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Models.Interfaces
{
    public interface IComment
    {
        Comment Find(int Id);
        void Remove(int Id);
        void Update(Comment comm);
        bool Add(Comment comm, string commenterId);
        //sIEnumerable<Comment> GetAll();
    }
}
