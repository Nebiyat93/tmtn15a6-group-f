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
    public class RecipesController : Controller
    {
        private Models.CommentManager s = new CommentManager();
        public RecipesController(IRecipe recep)
        {
            Recipes = recep;
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
            return Ok( new { item.Id, item.Name, item.CreatorId });
        }

        [HttpGet]
        public IActionResult GetNewest([FromQuery]int page)
        {
            var item = Recipes.GetAllSorted().Skip(2 * (page-1)).Take(2).ToList();
            if (page == 0 | item.Count == 0 )
                return NotFound();
            return Ok(item.Select(w => new { w.Id, w.Name, w.Created}));
        }

        [HttpGet("{id}/comments")]
        public IActionResult GetCommentsById(int Id)
        {
            var item = Recipes.Find(Id);
            if (item == null)
                return NotFound();
            return Ok(item.Comments.Select(w => new { w.Text, w.Grade, w.CommenterId }));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery]string term)
        {
            var item = Recipes.GetAll().Where(w => w.Name.Contains(term) || w.Description.Contains(term));
            //if ()
            //    return NotFound();
            return Ok(item.Select(w => new { w.Id, w.Name, w.Created }));
        }

        [HttpPost("recipes")]
        public IActionResult Create([FromBody] Recipe recep)
        {
            List<string> _errors = new List<string>();
            if (recep == null)
                return BadRequest();
            else if (string.IsNullOrWhiteSpace(recep.Name))
                _errors.Add("NameMissing");
            else if (recep.Name.Length < 5 || recep.Name.Length > 70)
                _errors.Add("NameWrongLength");
            else if (string.IsNullOrWhiteSpace(recep.Description))
                _errors.Add("DescriptionMissing");
            else if (recep.Description.Length < 10 && recep.Description.Length > 300)
                _errors.Add("DescriptionWrongLength");
            else if (recep.Directions == null)
                _errors.Add("DirectionsMissing");
            else if (string.IsNullOrWhiteSpace(recep.Directions.Select(w => w.Order).ToString()))
                _errors.Add("DirectionOrderMissing");
            else if (string.IsNullOrWhiteSpace(recep.Directions.Select(w => w.Description).ToString()))
                _errors.Add("DirectionDescriptionMissing");
            else
            Recipes.Add(recep);

            return CreatedAtRoute("GetRecep", new { id = recep.Id }, new { recep.Name, recep.Description, recep.CreatorId, recep.Directions});
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

        [HttpPost("{id}/comments")]
        public IActionResult CreateComment(int id, [FromBody] Comment comm)
        {
            if (comm == null)
                return BadRequest();

            // Create a comment --> implementation missing
            comm.RecipeId = id;
            s.Add(comm);

            return CreatedAtRoute("GetComm", new {  comm.Text, comm.Grade, comm.CommenterId });
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] Recipe newRecipe)
        {
            var oldRecipe = Recipes.Find(id);

            if (newRecipe == null || oldRecipe == null || oldRecipe.Id != id)
                return NotFound();

            // Error Messages missing!! (Bad Request)

            Recipes.Update(newRecipe, oldRecipe);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Auth missing.

            var acc = Recipes.Find(id);
            if (acc == null)
                return NotFound();

            Recipes.Remove(id);
            return new NoContentResult();
        }
    }
}
