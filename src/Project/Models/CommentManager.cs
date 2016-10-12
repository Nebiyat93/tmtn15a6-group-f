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
                comm.Created = RecipeManager.generateUnixTimestamp();
            } while (_context.Comments.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one

            var user = _context.Users.First(p => p.Id == comm.CommenterId);
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
            _context.Comments.Update(_comm);
            _context.SaveChanges();


            // Database is updated immediately - but "get recipe/comment" gets the old version until restart :(

            //var recep = _context.Recipes.First(r => r.Id == _comm.RecipeId);
            var recep = _context.Recipes.Include(u => u.Comments).First(r => r.Id == _comm.RecipeId);
            _context.Recipes.Update(recep);

            _context.SaveChanges();


        }
    }
}
