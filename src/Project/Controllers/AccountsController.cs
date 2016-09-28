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
            _logger = loggerFactory.CreateLogger<AccountsController>();
        }
        //public IAccount Accounts { get; set; }

        [HttpGet("GetAll")]
        public IEnumerable<AccountIdentity> GetAll()
        {
            return _userManager.Users;
        }

        [HttpGet("{id}", Name = "GetAcc")]
        public IActionResult GetById(string Id)
        {
            var item = _userManager.Users.FirstOrDefault(p => p.Id == Id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost("password")]
        public async Task<IActionResult> Create([FromBody] Account acc)
        {
            if (ModelState.IsValid)
            {
                var user = new AccountIdentity { UserName = acc.UserName, Latitude = acc.Latitude, Longitude = acc.Longitude };
                var res = await _userManager.CreateAsync(user, acc.Password);
                if (res.Succeeded)
                {
                    _logger.LogInformation(3, "Bla bla");
                    var item = _userManager.Users.FirstOrDefault(p => p.UserName == acc.UserName);
                    return CreatedAtRoute("GetAcc", new { id = item.Id }, acc);
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
           
            if (_userManager.Users.FirstOrDefault(p => p.Id == id) == null)
                return NotFound();
    
            if (acc.Latitude != null || acc.Longitude != null)
            {
                var _acc = _userManager.Users.First(p => p.Id == id);
                _acc.Longitude = acc.Longitude;
                _acc.Latitude = acc.Latitude;

                var res = await _userManager.UpdateAsync(_acc);
                if (res.Succeeded)
                {
                    _logger.LogInformation(3, "Bla bla");
                    return new NoContentResult();
                }

                foreach (var item in res.Errors)
                    _Errors.Add(item);
                return BadRequest(new { errors = _Errors });
            }
            else
            {
                return NoContent();
            }
            
            
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            
            var _acc = _userManager.Users.First(p => p.Id == id);
            if (_acc == null)
                return NotFound();

            var res = await _userManager.DeleteAsync(_acc);

            if (res.Succeeded)
            {
                _logger.LogInformation(3, "Bla bla");
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
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }


    }
}
