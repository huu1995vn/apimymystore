using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMyMyStore.DataAccess;
using APIMyMyStore.Entites;
using RaoXeAPI.Controllers;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CommonController
    {
        private readonly PostgreSqlContext _context;

        public UserController(PostgreSqlContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Getusers()
        {
            return _context.Users.ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            return Ok(() =>
            {
                var user = _context.Users.Find(id);

                if (user == null)
                {
                    throw new Exception(CommonConstants.MESSAGE_DATA_NOT_FOUND);
                }

                return user;
            });
           
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            return Ok(() =>
            {
                if (id != user.id && !UserExists(id))
                {
                    throw new Exception(CommonConstants.MESSAGE_DATA_NOT_FOUND);
                }

                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return NoContent();

            });
            
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<User> PostUser(User user)
        {
            return Ok(() =>
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return NoContent();
            });
           
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            return Ok(() =>
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
                return NoContent();
            });
           
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
