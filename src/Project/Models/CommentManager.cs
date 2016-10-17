using System;
using System.Collections.Generic;
using System.Linq;
using Project.Models.Interfaces;
using Project.SQL_Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Project.Models
{
    public class CommentManager : IComment
    {
       
        private readonly MyDbContext _context;

        public CommentManager(MyDbContext db )
        {
            this._context = db;
        }

        //public IEnumerable<Comment> GetAll()
        //{
        //    return _context.Comments;
        //}

        public bool Add(Comment comm, string commenterId)
        {

            var user = _context.Users.First(p => p.Id == commenterId);
            if (user != null)
            {
                int id;
                do
                {
                    id = Guid.NewGuid().GetHashCode(); //Returns numbers from GUID
                    if (id < 0)
                        id *= -1;
                    comm.Id = id;
                } while (_context.Comments.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one

                comm.Created = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                var recep = _context.Recipes.First(r => r.Id == comm.RecipeId);
                comm.AccountIdentity = user;

                _context.Comments.Add(comm);

                recep.Comments.Add(comm);
                user.Comments.Add(comm);


                _context.Users.Update(user);
                _context.Recipes.Update(recep);




                _context.SaveChanges();


                return true;
            }
            else return false;
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

            if (!string.IsNullOrWhiteSpace(comm.Text))
                _comm.Text = comm.Text;
            if (!string.IsNullOrWhiteSpace(comm.Image))
                _comm.Image = comm.Image;
            if (comm.Grade != 0)
                _comm.Grade = comm.Grade;
            _context.Comments.Update(_comm);
            _context.SaveChanges();
        }

    }
}
