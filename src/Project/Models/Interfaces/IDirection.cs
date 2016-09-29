
using System.Collections.Generic;

namespace Project.Models.Interfaces
{
    public interface IDirection
    {
        Direction Find(int Id);
        void Remove(int Id);
        void Update(Direction dir);
        void Add(Direction dir);
        IEnumerable<Direction> GetAll();
    }
}
