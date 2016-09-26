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
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private List<IdentityError> _Errors = new List<IdentityError>();
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly ILogger _logger;

        public AccountController(IAccount acc, 
            UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            ILoggerFactory loggerFactory)
        {
            Accounts = acc;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        public IAccount Accounts { get; set; }

        [HttpGet("GetAll")]
        public IEnumerable<Account> GetAll()
        {
            return Accounts.GetAll();
        }

        [HttpGet("{id}", Name = "GetAcc")]
        public IActionResult GetById(string Id)
        {
            var item = Accounts.Find(Id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Account acc)
        {
            if (ModelState.IsValid)
            {
                var user = new Account { UserName = acc.UserName, Email = acc.Email };
                var res = await _userManager.CreateAsync(user, acc.PasswordHash);
                if (res.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "Bla bla");
                    return Created("GetAcc", acc.Id);
                }

                foreach (var item in res.Errors)
                    _Errors.Add(item);
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Account acc)
        {
            if (acc.Longitude == null || acc.Latitude == null)
                return NoContent();
            else
            {
                var oldAcc = Accounts.Find  (id);
                if (acc == null || oldAcc.Id != id)
                    return NotFound();

                else if (oldAcc == null)
                    return NotFound();

                Accounts.Update(acc);
                return new NoContentResult();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var acc = Accounts.Find(id);
            if (acc == null)
                return NotFound();

            if (acc.Comments != null)
                

            Accounts.Remove(id);
            return new NoContentResult();
        }
    }
}
