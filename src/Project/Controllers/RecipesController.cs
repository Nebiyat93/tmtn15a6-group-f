using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Project.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class RecipesController : Controller
    {
        private IComment _commManager;
        public IUpload _imageHelp { get; set; }
        private IRecipe _recipes;
        private IUser _userService;
        private IValidateRecipe _validateRecipe;
        public RecipesController(IComment commentManager, 
            IRecipe recep, 
            IUpload imageHelp, 
            IUser userService, 
            IValidateRecipe validateRecipe)
        {
            _recipes = recep;
            _commManager = commentManager;
            _imageHelp = imageHelp;
            _userService = userService;
            _validateRecipe = validateRecipe;
        }
        
        /// <summary>
        /// Returns recipe information
        /// </summary>
        /// <param name="Id">Recipe ID</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetRecep")]
        public IActionResult GetById(int Id)
        {
            var item = _recipes.Find(Id);
            if (item == null)
                return NotFound();
            var creator = new { item.AccountIdentity.Id, item.AccountIdentity.UserName };
            var directions = item.Directions.Select(w => new { w.Order, w.Description });
            return Ok(new { item.Id, item.Name, item.Description, creator, item.Image, item.Created, directions });
        }

        [HttpGet]
        public IActionResult GetNewest([FromQuery]int page)
        {
            var item = _recipes.GetAllSorted().Skip(10 * (page - 1)).Take(10).ToList();
            if (page == 0 || item.Count == 0)
                return NotFound();
            return Ok(item.Select(w => new { w.Id, w.Name, w.Created }));
        }

        [HttpGet("{id}/comments")]
        public IActionResult GetCommentsById(int Id)
        {
            var item = _recipes.Find(Id);
            var comm = item.Comments;
            if (item == null)
                return NotFound();
            //commenter = new { w.CommenterId, w.AccountIdentity.UserName } // this code is what we should have when we get the comments to work.
            //var commenter = new { item.AccountIdentity.Id, item.AccountIdentity.UserName };
            var p = item.Comments.Select(w => new { w.Id, w.Text, w.Grade, commenter = new { w.CommenterId, w.AccountIdentity.UserName }, w.Image, w.Created });
            return Ok(p);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery]string term)
        {
            var item = _recipes.GetAll().Where(w => w.Name.Contains(term) || w.Description.Contains(term));
            //if ()
            //    return NotFound();
            return Ok(item.Select(w => new { w.Id, w.Name, w.Created }));
        }

        [HttpPost, Authorize]
        public IActionResult Create([FromBody] Recipe recep)
        {
            if (recep == null)
                return BadRequest();

            var thisUser = this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value;
            if (_userService.ValidateUser(thisUser))
            {
                if (_validateRecipe.ValidateProperties(recep, this.ModelState, HttpContext.Request.Method))
                {
                    _recipes.Add(recep, _userService.getCurrentUser(thisUser));
                    var direc = recep.Directions.Select(w => new { w.Order, w.Description });
                    return CreatedAtRoute("GetRecep", new { id = recep.Id }, new { recep.Name, recep.Description, recep.CreatorId, recep.Id, direc });
                }
                return BadRequest(new { errors = ModelState.Values.SelectMany(w => w.Errors).Select(e => e.ErrorMessage)});
            }
            else return Unauthorized();
                
        }

        // for Image!!
        [HttpPut("{id}/image"), Authorize]
        public IActionResult UpdloadImage(int id, IFormFile image)
        {
            var uri = _imageHelp.Upload(image);
            var recep = _recipes.Find(id);

            if (uri == null)
            {
                return NotFound();
            }
            else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == recep.CreatorId)
            {
                recep.Image = uri.AbsoluteUri;
                Update(id, recep); // Update the recipe. 
                return NoContent();
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost("{id}/comments"), Authorize] //Auth can't be tested on frontend
        public IActionResult CreateComment(int id, [FromBody] Comment comm)
        {

            if (ModelState.IsValid)
            {
                var userId = this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value;
                comm.RecipeId = id;

                if (_recipes.Find(id).Comments.Any(w => w.CommenterId == userId))
                {
                    ModelState.AddModelError("Commented", "CommenterAlreadyComment");
                    return BadRequest(new { errors = ModelState["Commented"].Errors.Select(w => w.ErrorMessage) });
                }
                comm.CommenterId = userId;

                if (_commManager.Add(comm, userId))
                    return CreatedAtRoute("GetComm", new { comm.Text, comm.Grade, comm.CommenterId });
                else return Unauthorized();
            }
            return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage)) });
        }

        [HttpPatch("{id}"), Authorize]
        public IActionResult Update(int id, [FromBody] Recipe newRecipe)
        {
            if (newRecipe == null)
                return BadRequest();

            var oldRecipe = _recipes.Find(id);
            if (oldRecipe == null)
                return NotFound();

            if (_validateRecipe.ValidateProperties(newRecipe, this.ModelState, HttpContext.Request.Method))
            {
                if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == oldRecipe.CreatorId)
                {
                    _recipes.Update(newRecipe, oldRecipe);
                    return new NoContentResult();
                }
                else
                    return Unauthorized();
            }
            return BadRequest(new { errors = ModelState.Values.SelectMany(w => w.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            var acc = _recipes.Find(id);
            if (acc == null)
                return NotFound();
            else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == acc.CreatorId)
            {
                _recipes.Remove(id);
                return new NoContentResult();
            }
            else return Unauthorized();
        }
    }
}
