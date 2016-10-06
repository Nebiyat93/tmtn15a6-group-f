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

        [HttpGet("{id}", Name = "GetComm")]
        public IActionResult GetById(int Id)
        {
            var comm = Comments.Find(Id);
            if (comm == null)
                return NotFound();
            return new ObjectResult(comm);
        }


        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] Comment comm)
        {
            bool check = true;
            List<string> err = new List<string>();
            comm.Id = id;
            var p = Comments.Find(id);
            if (p == null)
            {
                return NotFound();
            }
            if (comm.Text.Length < 10 || comm.Text.Length >400)
            {
                err.Add("TextWrongLength");
                check = false;
            }
            if(comm.Grade <1 || comm.Grade >5)
            {
                err.Add("GradeWrongValue");
                check = false;
            }
            if (check)
            {
                Comments.Update(comm);
                return new NoContentResult();
            }
            else
                return BadRequest(new { error = err});


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
