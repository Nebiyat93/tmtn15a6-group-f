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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class RecipesController : Controller
    {
        private IComment CommManager;
        public IUpload imageHelp { get; set; }
        public RecipesController(IComment commentManager, IRecipe recep, IUpload imageHelp)
        {
            Recipes = recep;
            this.CommManager = commentManager;
            this.imageHelp = imageHelp;
        }
        public IRecipe Recipes { get; set; }

        /// <summary>
        /// Returns recipe information
        /// </summary>
        /// <param name="Id">Recipe ID</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetRecep")]
        public IActionResult GetById(int Id)
        {
            var item = Recipes.Find(Id);
            if (item == null)
                return NotFound();
            var creator = new { item.AccountIdentity.Id, item.AccountIdentity.UserName };
            var directions = item.Directions.Select(w => new { w.Order, w.Description });
            return Ok(new { item.Id, item.Name, item.Description, creator, item.Image, item.Created, directions});
        }

        [HttpGet]
        public IActionResult GetNewest([FromQuery]int page)
        {
            var item = Recipes.GetAllSorted().Skip(10 * (page - 1)).Take(10).ToList();
            if (page == 0 || item.Count == 0)
                return NotFound();
            return Ok(item.Select(w => new { w.Id, w.Name, w.Created }));
        }

        [HttpGet("{id}/comments")]
        public IActionResult GetCommentsById(int Id)
        {
            var item = Recipes.Find(Id);
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
            var item = Recipes.GetAll().Where(w => w.Name.Contains(term) || w.Description.Contains(term));
            //if ()
            //    return NotFound();
            return Ok(item.Select(w => new { w.Id, w.Name, w.Created }));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Recipe recep) //Cannot add direction????
        {
            if (ModelState.IsValid)
            {
                Recipes.Add(recep, this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value);
                var direc = recep.Directions.Select(w => new { w.Order, w.Description });
                return CreatedAtRoute("GetRecep", new { id = recep.Id }, new { recep.Name, recep.Description, recep.CreatorId, recep.Id, direc });
            }
            else
                return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage)) });
        }

        // for Image!!
        [HttpPut("{id}/image"),Authorize]
        public IActionResult UpdloadImage(int id, IFormFile image)
        {
            var uri = imageHelp.Upload(image);
            var recep = Recipes.Find(id);
            
            if (uri == null) {
                return NotFound();
            }
            else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == recep.CreatorId )
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

                if (Recipes.Find(id).Comments.Any(w=>w.CommenterId == userId))
                {
                    ModelState.AddModelError("Commented", "CommenterAlreadyComment");
                    return BadRequest(new { errors = ModelState["Commented"].Errors.Select(w => w.ErrorMessage)});
                }
                comm.CommenterId = userId;

                if (CommManager.Add(comm, userId))
                    return CreatedAtRoute("GetComm", new { comm.Text, comm.Grade, comm.CommenterId });
                else return Unauthorized();
            }
            return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage)) });
        }

        [HttpPatch("{id}"), Authorize]
        public IActionResult Update(int id, [FromBody] Recipe newRecipe)
        {
            if (string.IsNullOrWhiteSpace(newRecipe.Name))
                ModelState.Remove("Name");
            if (string.IsNullOrWhiteSpace(newRecipe.Description))
                ModelState.Remove("Description");
            if (newRecipe.Directions == null || newRecipe.Directions.Count == 0)
                ModelState.Remove("Directions");
            else
            {
                if (string.IsNullOrWhiteSpace(newRecipe.Directions.Select(w => w.Order).ToString()))
                    ModelState.Remove("Order");
                if (string.IsNullOrWhiteSpace(newRecipe.Directions.Select(w => w.Description).ToString()))
                    ModelState.Remove("Description");
            }

            if (ModelState.IsValid)
            {
                var oldRecipe = Recipes.Find(id);
                if (newRecipe == null || oldRecipe == null || oldRecipe.Id != id)
                    return NotFound();
                else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == oldRecipe.CreatorId)
                {
                    Recipes.Update(newRecipe, oldRecipe);
                    return new NoContentResult();
                }
                else
                    return Unauthorized();
                
            }
            return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage))});
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            var acc = Recipes.Find(id);
            if (acc == null)
                return NotFound();
            else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == acc.CreatorId)
            {
                Recipes.Remove(id);
                return new NoContentResult();
            }
            else return Unauthorized();
        }
    }
}
