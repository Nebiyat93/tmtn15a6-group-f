using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Project.SQL_Database;
using Project.Models.Interfaces;
using System;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountsController : Controller
    {
        private readonly UserManager<AccountIdentity> _userManager;
        public IUpload imageHelp { get; set; }
        public AccountsController(UserManager<AccountIdentity> userManager, IUpload imageHelp)
        {
            _userManager = userManager;
            this.imageHelp = imageHelp;
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
            return Ok(new { item.Id, item.UserName, item.Latitude, item.Longitude });
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

                    return CreatedAtRoute("GetAcc", new { id = item.Id }, new { item.Id, item.UserName, item.Longitude, item.Latitude });
                }
                return BadRequest(new { errors = res.Errors });
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="acc"></param>
        /// <returns></returns>
        [HttpPatch("{id}"), Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] Account acc)
        {
            var _acc = _userManager.Users.FirstOrDefault(p => p.Id == id);
            if (_acc != null)
            {
                if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == _acc.Id)
                {
                    //A user cannot change username or password
                    if (acc.UserName != null || acc.Password != null)
                        return Unauthorized();

                    if (acc.Longitude != null)
                        _acc.Longitude = acc.Longitude;
                    if (acc.Latitude != null)
                        _acc.Latitude = acc.Latitude;

                    var res = await _userManager.UpdateAsync(_acc);
                    if (res.Succeeded)
                        return NoContent();
                }
                else return Unauthorized();
            }
            return NotFound();
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var _acc = _userManager.Users.Include(r=>r.Recipes).Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            if (this.User.Claims.FirstOrDefault(w => w.Type == "userId").Value == _acc.Id)
            {
                var res = await _userManager.DeleteAsync(_acc);
                removeOrphans(_acc);
                if (res.Succeeded) 
                    return NoContent();
                else return BadRequest(new { errors = res.Errors });
            }
            else
                return Unauthorized();
        }

        public void removeOrphans(AccountIdentity parent)
        {
            MyDbContext _context = new MyDbContext();


            //var recep = _context.Recipes.Include(u => u.AccountIdentity).Include(u => u.Comments).Include(d => d.Directions).ToList().TakeWhile(p => p.CreatorId == parent.Id);
            //_context.Comments.Where(p => p.AccountIdentity == null).ToList().ForEach(c => _context.Comments.Remove(c));
            foreach(var comm in _context.Comments.Where(p => p.AccountIdentity == null).Include(r=>r.Recipe).Include(s=>s.AccountIdentity))
            {
                if (comm.Image != null)
                {
                    var im = new Uri(comm.Image);
                    var name = im.Segments[im.Segments.Length - 1];
                    var test = imageHelp.Remove(name);
                }

                _context.Remove(comm);
            }
            // Loop through the recieps with accountidentity == null.
            foreach(var recep in _context.Recipes.Where(p => p.AccountIdentity == null).Include(c=>c.Comments).Include(s=>s.AccountIdentity))
            {
                if (recep.Image != null)
                {
                    var im = new Uri(recep.Image);
                    var name = im.Segments[im.Segments.Length - 1];
                    var test = imageHelp.Remove(name);
                }
                
                _context.Remove(recep);
            }
            //_context.Recipes.Where(p => p.AccountIdentity == null).ToList().ForEach(r => _context.Recipes.Remove(r));
           _context.SaveChanges();

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
