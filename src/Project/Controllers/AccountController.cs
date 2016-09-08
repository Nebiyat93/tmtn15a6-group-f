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
    public class AccountController : Controller
    {

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
            if (acc == null)
                return BadRequest();

            if (acc.UserName.Length > 0 && acc.UserName.Length < 13)
            {

                if (m_Accounts.Count < 1)
                {
                    m_Accounts.Add(acc);
                    Accounts.Add(acc);
                    return Ok();
                }
                else
                {
                    for (int i = 0; i < m_Accounts.Count; i++)
                    {
                        if (m_Accounts[i].UserName != acc.UserName)
                        {
                            m_Accounts.Add(acc);
                            Accounts.Add(acc);
                            return Ok();
                        }
                        else return BadRequest();
                    }
                }
            }
            else return BadRequest();

            return CreatedAtRoute("GetAcc", new { id = acc.Id }, acc);
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
    }
}
