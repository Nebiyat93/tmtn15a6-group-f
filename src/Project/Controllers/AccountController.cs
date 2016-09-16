using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
using Project.SQL_Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        public static List<Account> m_Accounts = new List<Account>();
        private List<BadRequestObjectResult> _badRequests = new List<BadRequestObjectResult>();

        public AccountController(IAccount acc)
        {
            Accounts = acc;
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
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Account acc)
        {
            try
            {
                if (CheckInputs(acc))
                    return CreatedAtRoute("GetAcc", new { id = acc.Id }, acc);
                else
                    return BadRequest(_badRequests.Select(p => p.Value.ToString())); //Returns every error from _badRequests as an array/list
            }
            finally
            {
                _badRequests.Clear();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Account acc)
        {
            if (acc == null || acc.Id != id)
            {
                return BadRequest();
            }

            var p = Accounts.Find(id);
            if (p == null)
            {
                return NotFound();
            }

            Accounts.Update(acc);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var acc = Accounts.Find(id);
            if (acc == null)
            {
                return NotFound();
            }

            Accounts.Remove(id);
            return new NoContentResult();
        }
        private bool CheckInputs(Account acc)
        {
            if (acc == null)
            {
                _badRequests.Add(BadRequest("NullAccount"));
                return false;
            }

            if (acc.UserName == null)
            {
                _badRequests.Add(BadRequest("UserNameMissing"));
                return false;
            }

            else
            {
                if (Accounts.GetAll() == null)
                {
                    Accounts.Add(acc);
                    return true;
                }

                else
                {
                    if (Accounts.FindUser(acc.UserName) != null)
                    {
                        _badRequests.Add(BadRequest("DuplicateUserName"));
                        return false;
                    }
                    if (ModelState.IsValid) // Does check with the DataAnnotations if its true we do all other checks that the database couldnt handle.
                        Accounts.Add(acc);
                    else
                    {
                        _badRequests.Add(BadRequest("ModelStateNotValid"));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
