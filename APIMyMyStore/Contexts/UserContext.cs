using APIMyMyStore.Entites;
using Microsoft.EntityFrameworkCore;

namespace APIMyMyStore.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
