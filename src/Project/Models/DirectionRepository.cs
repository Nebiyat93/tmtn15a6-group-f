using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.Models;


namespace Project.Models
{
    public class DirectionRepository : IDirection
    {
        private static ConcurrentDictionary<int, Direction> _dir =
      new ConcurrentDictionary<int, Direction>();


        public DirectionRepository()
        {
            Add(new Direction { Id = 0 });
        }

        public IEnumerable<Direction> GetAll()
        {
            return _dir.Values;
        }

        public void Add(Direction dir)
        {
            dir.Id = 0;
            _dir[dir.Id] = dir;
        }

        public Direction Find(int id)
        {
            Direction dir;
            _dir.TryGetValue(id, out dir);
            return dir;
        }

        public Direction Remove(int id)
        {
            Direction dir;
            _dir.TryRemove(id, out dir);
            return dir;
        }

        public void Update(Direction dir)
        {
            _dir[dir.Id] = dir;
        }
    }

}
