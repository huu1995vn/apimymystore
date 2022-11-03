using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMyMyStore.DataAccess;
using APIMyMyStore.Entites;
using RaoXeAPI.Controllers;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : CommonController
    {
        private readonly PostgreSqlContext _context;

        public AdminController(PostgreSqlContext context)
        {
            _context = context;
        }

        // GET: api/Admins
        [HttpGet]
        public ActionResult<IEnumerable<Admin>> Getadmins()
        {
            return FormatListAdmin(_context.Admins.ToList());
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public ActionResult<Admin> GetAdmin(int id)
        {
            return Ok(() =>
            {
                var admin = _context.Admins.Find(id);

                if (admin == null)
                {
                    throw new Exception(CommonConstants.MESSAGE_DATA_NOT_FOUND);
                }

                return FormatAdmin(admin);
            });

        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutAdmin(int id, Admin admin)
        {
            return Ok(() =>
            {
                if (id != admin.id && !AdminExists(id))
                {
                    throw new Exception(CommonConstants.MESSAGE_DATA_NOT_FOUND);
                }

                _context.Entry(admin).State = EntityState.Modified;
                _context.SaveChanges();
                return NoContent();
                //return CreatedAtAction("GetAdmin", new { id = admin.id }, admin);

            });

        }

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Admin> PostAdmin(Admin admin)
        {
            return Ok(() =>
            {
                _context.Admins.Add(admin);
                _context.SaveChanges();
                return NoContent();
                //return CreatedAtAction("GetAdmin", new { id = admin.id }, admin);
            });

        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(int id)
        {
            return Ok(() =>
            {
                var admin = _context.Admins.Find(id);
                if (admin == null)
                {
                    return NotFound();
                }

                _context.Admins.Remove(admin);
                _context.SaveChanges();
                return NoContent();
            });

        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.id == id);
        }

        private Admin FormatAdmin(Admin admin)
        {
            admin.password = null;
            admin.token = null;
            return admin;
        }

        private List<Admin> FormatListAdmin(List<Admin> ladmin)
        {
       
            return ladmin.Select(item=> FormatAdmin(item)).ToList();
        }
    }
}
