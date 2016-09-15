using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models.Interfaces;
using Project.Models;
using Project.SQL_Database;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.Controllers
{
    [Route("api/v1/[controller]")]
    public class AccountController : Controller
    {
        private MyDbContext _context = new MyDbContext();
        public static List<Account> m_Accounts = new List<Account>();

        

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
            //string errors = "";

            

            if (acc == null)
            {
                //errors = BadRequest().ToString();
                return BadRequest();
            }

            if (acc.UserName == null)
            {
                //errors = errors + BadRequest("UserNameMissing").ToString();
                return BadRequest("UserNameMissing");
            }

            if (acc.Longitude == 0)
                return BadRequest("LongitudeMissing");

            if (acc.Latitude == 0)
                return BadRequest("LatitudeMissing");

            if (acc.UserName.Length < 0 && acc.UserName.Length > 13)
                return BadRequest("InvalidUserName");

            else
            {
                if (Accounts.GetAll() == null)

                {
                    Accounts.Add(acc);
                    return Created(acc.Id, acc);
                }

                else
                {
                    if (Accounts.FindUser(acc.UserName) != null)
                        return BadRequest("DuplicateUserName");

                    if (ModelState.IsValid) // Does check with the DataAnnotations if its true we do all other checks that the database couldnt handle.
                    {

                        Accounts.Add(acc);
                        return CreatedAtRoute("GetAcc", new { id = acc.Id }, acc);

                        //return Ok();
                    }

                    else
                    {
                        //Accounts.Add(acc);
                        //return Created(acc.Id, acc);
                        return BadRequest();
                    }

                }

            }
        }
                
            /*
            else (errors != "")
            {
                    return BadRequest("errors:" + errors);
                }
            }*/

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
    }
}
