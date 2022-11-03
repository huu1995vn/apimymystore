using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMyMyStore.Entites;
using APIMyMyStore.DataAccess;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public UsersController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _dataAccessProvider.GetUserRecords();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _dataAccessProvider.GetUserSingleRecord(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public IActionResult PutUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                if (!UserExists(user.id))
                {
                    return NotFound();
                }
                _dataAccessProvider.UpdateUserRecord(user);
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<User> PostUser([FromBody] User user)
        {

            if (ModelState.IsValid)
            {
                _dataAccessProvider.AddUserRecord(user);
                return Ok();
            }
            return BadRequest();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }
            _dataAccessProvider.DeleteUserRecord(id);
            return Ok();
        }

        private bool UserExists(int id)
        {
            return _dataAccessProvider.GetUserSingleRecord(id) != null;
        }
    }
}
