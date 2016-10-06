using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class CommentController : Controller
    {
        public CommentController(IComment comm)
        {
            Comments = comm;
        }
        public IComment Comments { get; set; }

        [HttpGet("GetAll")]
        public IEnumerable<Comment> GetAll()
        {
            return Comments.GetAll();
        }

        [HttpGet("{id}", Name = "GetComm")]
        public IActionResult GetById(int Id)
        {
            var comm = Comments.Find(Id);
            if (comm == null)
                return NotFound();
            return new ObjectResult(comm);
        }

        //[HttpPost]
        //public IActionResult Create([FromBody] Comment comm)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        if (string.IsNullOrWhiteSpace((comm.Text)))
        //        {
        //            return BadRequest();
        //        }
        //        else
        //        {
        //            Comments.Add(comm);
        //            return CreatedAtRoute("GetComm", new { id = comm.Id }, comm);
        //        }
        //    }else
        //    {
        //        return BadRequest();
        //    }
            
        //}

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Comment comm)
        {
            if (comm == null || comm.Id != id)
            {
                return BadRequest();
            }

            var p = Comments.Find(id);
            if (p == null)
            {
                return NotFound();
            }

            Comments.Update(comm);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var comm = Comments.Find(id);
            if (comm == null)
                return NotFound();

            Comments.Remove(id);
            return new NoContentResult();
        }
    }
}
