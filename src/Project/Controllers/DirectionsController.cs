using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class DirectionsController : Controller
    {
        public DirectionsController(IDirection dir)
        {
            Directions = dir;
        }
        public IDirection Directions { get; set; }

        [HttpGet]
        public IEnumerable<Direction> GetAll()
        {
            return Directions.GetAll();
        }

        [HttpGet("{id}", Name = "GetDir")]
        public IActionResult GetById(int Id)
        {
            var dir = Directions.Find(Id);
            if (dir == null)
                return NotFound();
            return new ObjectResult(dir);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Direction dir)
        {
            if (dir == null)
            {
                return BadRequest();
            }
            Directions.Add(dir);
            return CreatedAtRoute("GetDir", new { id = dir.Id }, dir);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Direction dir)
        {
            if (dir == null || dir.Id != id)
            {
                return BadRequest();
            }

            var p = Directions.Find(id);
            if (p == null)
            {
                return NotFound();
            }

            Directions.Update(dir);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dir = Directions.Find(id);
            if (dir == null)
                return NotFound();

            Directions.Remove(id);
            return new NoContentResult();
        }

    }
}
