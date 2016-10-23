using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Models.Interfaces;
using System.Collections.Concurrent;
using Project.Models;
using Project.SQL_Database;



namespace Project.Models
{
    public class DirectionManager : IDirection
    {

        private readonly MyDbContext _context;

        public DirectionManager(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Direction> GetAll()
        {
            return _context.Directions;
        }

        public void Add(Direction dir)
        {
            int id;
            do
            {
                id = Guid.NewGuid().GetHashCode(); //Returns numbers from GUID
                if (id < 0)
                    id *= -1;
                dir.Id = id;
   
            } while (_context.Directions.Any(h => h.Id == id)); //Loops as long as the existing row's id is the same as the newly generated one
            _context.Directions.Add(dir);
            var recep = _context.Recipes.First(p => p.Id == dir.RecipeId);
            recep.Directions.Add(dir);
            _context.Recipes.Update(recep);
            _context.SaveChanges();
        }

        public Direction Find(int id)
        {
            return _context.Directions.Where(h => h.Id == id).FirstOrDefault();
        }

        public void Remove(int id)
        {
            var dir = Find(id);
            _context.Remove(dir);
            _context.SaveChanges();
        }

        public void Update(Direction dir)
        {
            
        }
    }

}
