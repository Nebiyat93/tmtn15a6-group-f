using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
using Project.SQL_Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountsController : Controller
    {
        private MyDbContext _context = new MyDbContext();
        private List<IdentityError> _Errors = new List<IdentityError>();
        private readonly UserManager<AccountIdentity> _userManager;
        private readonly ILogger _logger;
        public AccountsController( UserManager<AccountIdentity> userManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager; 
        }

        //[HttpGet("GetAll")]
        //public IEnumerable<AccountIdentity> GetAll()
        //{
        //    var users = _userManager.Users.Include(u => u.Recipes).ToList();
        //    return users;
        //    var u = _userManager.Users
        //}

        [HttpGet("{id}", Name = "GetAcc")]
        public IActionResult GetById(string Id)
        {
            var item = _userManager.Users.Include(u => u.Recipes).ToList().FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            return Ok(new {item.Id, item.UserName, item.Latitude, item.Longitude});
        }

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
                    _logger.LogInformation(3, "Bla bla");
                    var item = _userManager.Users.FirstOrDefault(p => p.UserName == acc.UserName);
                    
                    return CreatedAtRoute("GetAcc", new { id = item.Id }, new { item.Id, item.UserName, item.Longitude, item.Latitude } );
                }

                
                foreach (var item in res.Errors)
                    _Errors.Add(item);
                return BadRequest(new { errors = _Errors });
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

                foreach (var item in res.Errors)
                    _Errors.Add(item);
                return BadRequest(new { errors = _Errors });
            }

            return BadRequest();    
            
            
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        { 
            // Auth missing.
            
            var _acc = _userManager.Users.First(p => p.Id == id);
            if (_acc == null)
                return NotFound();

            var res = await _userManager.DeleteAsync(_acc);

            if (res.Succeeded)
            {
                return new NoContentResult();
            }
            foreach (var item in res.Errors)
                _Errors.Add(item);
            return BadRequest(new { errors = _Errors });
            
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
