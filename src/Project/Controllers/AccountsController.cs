using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountsController : Controller
    {
        private readonly UserManager<AccountIdentity> _userManager;

        public AccountsController(UserManager<AccountIdentity> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Returns user information
        /// </summary>
        /// <param name="Id">User's ID</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetAcc")]
        public IActionResult GetById(string Id)
        {
            var item = _userManager.Users.Include(u => u.Recipes).ToList().FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            return Ok(new {item.Id, item.UserName, item.Latitude, item.Longitude});
        }

        /// <summary>
        /// Returns recipe(s) that's bound to the ID
        /// </summary>
        /// <param name="Id">User's ID</param>
        /// <returns></returns>
        [HttpGet("{id}/recipes")]
        public IActionResult GetRecipesById(string Id)
        {
            var item = _userManager.Users.Include(u => u.Recipes).ToList().FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            return Ok(item.Recipes.Select(w => new { w.Id, w.Name, w.Created }));
        }


        [HttpGet("{id}/comments")]
        public IActionResult GetCommentsById(string Id)
        {
            var item = _userManager.Users.Include(u => u.Comments).ToList().FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            return Ok(item.Comments.Select(w => new { w.Id, w.RecipeId, w.Text, w.Grade, w.Created }));
        }

        [HttpGet("{id}/favorites")]
        public IActionResult GetFavoritesById(string Id)
        {
            var item = _userManager.Users.Include(u => u.Favorites).ToList().FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            return Ok(item.Favorites.Select(w => new { w.RecipeId, w.Recipe.Name, w.Recipe.Created }));
        }

        [HttpPut("{id}/favorites")]
        public IActionResult UpdateFavoritesById(string Id)
        {
            var item = _userManager.Users.Include(u => u.Favorites).ToList().FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            
            return Ok(item.Favorites.Select(w => new { w.RecipeId, w.Recipe.Name, w.Recipe.Created }));
        }

        
        
        [HttpPost("password")]
        public async Task<IActionResult> Create([FromBody] Account acc)
        {
            if (ModelState.IsValid)
            {
                List<string> err = new List<string>();

                if (acc.Longitude == null)
                    err.Add("LongitudeMissing");
                if (acc.Latitude == null)
                    err.Add("LatitudeMissing");
                if (err.Count > 0)
                    return BadRequest(new { error = err });


                var user = new AccountIdentity { UserName = acc.UserName, Latitude = acc.Latitude, Longitude = acc.Longitude };
                var res = await _userManager.CreateAsync(user, acc.Password);
                if (res.Succeeded)
                {
                    var item = _userManager.Users.FirstOrDefault(p => p.UserName == acc.UserName);
                    
                    return CreatedAtRoute("GetAcc", new { id = item.Id }, new { item.Id, item.UserName, item.Longitude, item.Latitude } );
                }
                return BadRequest(new { errors = res.Errors });
            }
            return BadRequest();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Account acc)
        {
            var _acc = _userManager.Users.First(p => p.Id == id);

            if (_acc == null)
                return NotFound();
    
            if (acc.Latitude != null || acc.Longitude != null)
            {
                
                if(acc.Longitude != null)
                    _acc.Longitude = acc.Longitude;
                if(acc.Latitude != null)
                    _acc.Latitude = acc.Latitude;

                var res = await _userManager.UpdateAsync(_acc);
                if (res.Succeeded)
                {
                    return NoContent();
                }
                return BadRequest(new { errors = res.Errors });
            }

            return BadRequest();    
            
            
           
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            // Auth missing.

            if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == _userManager.Users.FirstOrDefault(w => w.Id == id).ToString())
            {
                return Ok();
            }

            var _acc = _userManager.Users.First(p => p.Id == id);
            if (_acc == null)
                return NotFound();

            var res = await _userManager.DeleteAsync(_acc);

            if (res.Succeeded)
                return new NoContentResult();
            return BadRequest(new { errors = res.Errors });
        }
    }
    public class Account
    {
        [Key]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
