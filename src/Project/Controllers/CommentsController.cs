using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class CommentsController : Controller
    {
        public IComment Comments { get; set; }
        public IUpload imageHelp { get; set; }
        public CommentsController(IComment comm, IUpload imageHelp)
        {
            Comments = comm;
            this.imageHelp = imageHelp;
        }


        [HttpGet("{id}", Name = "GetComm")]
        public IActionResult GetById(int Id)
        {
            var comm = Comments.Find(Id);
            if (comm == null)
                return NotFound();
            return new ObjectResult(comm);
        }


        [HttpPatch("{id}"), Authorize]
        public IActionResult Update(int id, [FromBody] Comment comm)
        {
            if (string.IsNullOrWhiteSpace(comm.Text))
                ModelState.Remove("Text");
            if (comm.Grade == 0)
                ModelState.Remove("Grade");

            if (ModelState.IsValid)
            {
                comm.Id = id;
                var p = Comments.Find(id);
                if (p == null)
                    return NotFound();
                else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == p.CommenterId)
                {
                    Comments.Update(comm);
                    return new NoContentResult();
                }
                else
                    return Unauthorized();
            }
            return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage)) });
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
        [HttpPut("{id}/image"), Authorize]
        public IActionResult UpdloadImage(int id, IFormFile image)
        {
            var uri = imageHelp.Upload(image);
            var item = Comments.Find(id);

            if (uri == null)
            {
                return NotFound();
            }
            else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == item.CommenterId) // CHECK THIS!!! Doenst work. upload works even if ur not signed in.
            {
                item.Image = uri.AbsoluteUri;
                Update(id, item); // Update the recipe. 
                return NoContent();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
