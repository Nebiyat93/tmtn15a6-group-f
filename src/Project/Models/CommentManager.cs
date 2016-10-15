using System;
using System.Collections.Generic;
using System.Linq;
using Project.Models.Interfaces;
using Project.SQL_Database;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class CommentManager : IComment
    {

        private readonly MyDbContext _context;
        public CommentManager(MyDbContext db)
        {
            this._context = db;
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public void Add(Comment comm, string commenterId)
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode(); //Returns numbers from GUID
                if (id < 0)
                    id *= -1;
                comm.Id = id;
                comm.Created = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            } while (_context.Comments.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one

            var user = _context.Users.First(p => p.Id == commenterId);
            user.Comments.Add(comm);
            _context.Users.Update(user);
            _context.Comments.Add(comm);
            _context.SaveChanges();

            var recep = _context.Recipes.First(r => r.Id == comm.RecipeId);
            recep.Comments.Add(comm);
            _context.Recipes.Update(recep);
            
          
        }

        public Comment Find(int id)
        {
            return _context.Comments.FirstOrDefault(_id=>_id.Id == id);
            
        }

        public void Remove(int id)
        {
            _context.Comments.Remove(Find(id));
            _context.SaveChanges();
            
        }

        public void Update(Comment comm)
        {
            var _comm = Find(comm.Id);
            _comm.Text = comm.Text;
            _comm.Grade = comm.Grade;
            _context.SaveChanges();


        }
    }
}
