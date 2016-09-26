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
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private List<string> _Errors = new List<string>();
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public AccountController(IAccount acc, 
            UserManager<Account> userManager,
            SignInManager<Account> signInManager)
        {
            Accounts = acc;
            _userManager = userManager;
            _signInManager = signInManager;
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
            var user = new Account { UserName = acc.UserName, Email = acc.Email };
            return Ok();

            //if (CheckInputs(acc))
            //    return CreatedAtRoute("GetAcc", new { id = acc.Id }, acc);
            //else
            //    return BadRequest(new { errors = _Errors });


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
        private bool CheckInputs(Account acc)
        {
            if (acc == null)
                return false;

            if (acc.UserName == null)
                _Errors.Add("UserNameMissing");
            if (acc.UserName.Contains(" "))
                _Errors.Add("InvalidUserName");
            if (acc.Longitude == null)
                _Errors.Add("LongitudeMissing");
            if (acc.Latitude == null)
                _Errors.Add("LatitudeMissing");
            else
            {   
                if (!Accounts.GetAll().Any())
                    Accounts.Add(acc);
                else
                {
                    if (Accounts.Find(acc.UserName) != null)
                    {
                        _Errors.Add("DuplicateUserName  ");
                        return false;
                    }
                    if (ModelState.IsValid) // Does check with the DataAnnotations if its true we do all other checks that the database couldnt handle.
                        Accounts.Add(acc);
                    else
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
