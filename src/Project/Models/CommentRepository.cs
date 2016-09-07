using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;

namespace Project.Models
{
    public class CommentRepository
    {
        private static ConcurrentDictionary<int, Comment> _comm =
      new ConcurrentDictionary<int, Comment>();

        public CommentRepository()
        {
            Add(new Comment { Id = 0 });
        }

        public IEnumerable<Comment> GetAll()
        {
            return _comm.Values;
        }

        public void Add(Comment comm)
        {
            comm.Id = 0;
            _comm[comm.Id] = comm;
        }

        public Comment Find(int id)
        {
            Comment comm;
            _comm.TryGetValue(id, out comm);
            return comm;
        }

        public Comment Remove(int id)
        {
            Comment comm;
            _comm.TryRemove(id, out comm);
            return comm;
        }

        public void Update(Comment comm)
        {
            _comm[comm.Id] = comm;
        }
    }
}
