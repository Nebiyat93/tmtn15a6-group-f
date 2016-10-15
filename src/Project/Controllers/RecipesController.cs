using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class RecipesController : Controller
    {
        private IComment CommManager;

        public RecipesController(IComment commentManager, IRecipe recep){
            Recipes = recep;
            this.CommManager = commentManager;
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
            if (item == null)
                return NotFound();
            var commenter = new { item.AccountIdentity.Id, item.AccountIdentity.UserName };
            var p = item.Comments.Select(w => new { w.Text, w.Grade, commenter, w.Id, w.Created });
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
                return CreatedAtRoute("GetRecep", new { id = recep.Id }, new { recep.Name, recep.Description, recep.CreatorId, recep.Directions });
            }
            else
                return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage)) });
        }

        // for Image!!
        [HttpPut("{id}/image")]
        public IActionResult UpdloadImage(int id, [FromBody] Recipe newRecipe)
        {
            var oldRecipe = Recipes.Find(id);
            if (newRecipe == null || oldRecipe.Id != id)
                return NotFound();

            else if (oldRecipe == null)
                return NotFound();

            Recipes.Update(newRecipe, oldRecipe);
            return new NoContentResult();
        }

        [HttpPost("{id}/comments"), Authorize]
        public IActionResult CreateComment(int id, [FromBody] Comment comm)
        {
            if (ModelState.IsValid)
            {
                comm.RecipeId = id;
                var userId = this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value;
                CommManager.Add(comm, userId);
                return CreatedAtRoute("GetComm", new { comm.Text, comm.Grade, comm.CommenterId });
            }
            else return BadRequest();

        }

        [HttpPatch("{id}"), Authorize] //NOT FINISHED, not able to update direction!
        public IActionResult Update(int id, [FromBody] Recipe newRecipe)
        {
            if (ModelState.IsValid)
            {
                var oldRecipe = Recipes.Find(id);
                if (oldRecipe == null || oldRecipe.Id != id)
                    return NotFound();
                else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == oldRecipe.CreatorId)
                {
                    Recipes.Update(newRecipe, oldRecipe);
                    return new NoContentResult();
                }
                else return Unauthorized();
            }
            else return BadRequest(new { errors = ModelState.Values.Select(w => w.Errors.Select(p => p.ErrorMessage))});
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            var acc = Recipes.Find(id);
            if (acc == null)
                return NotFound();
            else if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == id.ToString())
            {
                Recipes.Remove(id);
                return new NoContentResult();
            }
            else return Unauthorized();
        }
    }
}
