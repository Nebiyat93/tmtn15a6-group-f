using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.SQL_Database;

namespace Project.Models
{
    public class CommentRepository : IComment
    {

        private MyDbContext _context = new MyDbContext();
      

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public void Add(Comment comm)
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode(); //Returns numbers from GUID
                if (id < 0)
                    id *= -1;
                comm.Id = id;
                comm.Created = RecipeRepository.generateUnixTimestamp();
            } while (_context.Comments.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one

            _context.Comments.Add(comm);
            _context.SaveChanges();
        }

        public Comment Find(int id)
        {
            return _context.Comments.First(_id=>_id.Id == id);
            
        }

        public void Remove(int id)
        {
            _context.Comments.Remove(Find(id));
            _context.SaveChanges();
            
        }

        public void Update(Comment comm)
        {
            var _comm = Find(comm.Id);
            _comm.Id = comm.Id;
            _comm.Text = comm.Text;
            _comm.Grade = comm.Grade;
            _comm.Image = comm.Image;
            _comm.Created = comm.Created;
        }
    }
}
