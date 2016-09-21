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
    public class RecipeController : Controller
    {
        public RecipeController(IRecipe recep)
        {
            Recipes = recep;
        }
        public IRecipe Recipes { get; set; }

        [HttpGet("GetAll")]
        public IEnumerable<Recipe> GetAll()
        {
            return Recipes.GetAll();
        }

        [HttpGet("{id}", Name = "GetRecep")]
        public IActionResult GetById(int Id)
        {
            var item = Recipes.Find(Id);
            if (item == null)
                return NotFound();
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Recipe recep)
        {
            if (recep == null)
            {
                return BadRequest();
            }
            Recipes.Add(recep);
            return CreatedAtRoute("GetRecep", new { id = recep.Id }, recep);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Recipe newRecipe)
        {
            var oldRecipe = Recipes.Find(id);
            if (newRecipe == null || oldRecipe.Id != id)
                return NotFound();

            if (oldRecipe == null)
                return NotFound();

            Recipes.Update(newRecipe, oldRecipe);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var acc = Recipes.Find(id);
            if (acc == null)
            {
                return NotFound();
            }

            Recipes.Remove(id);
            return new NoContentResult();
        }
    }
}
