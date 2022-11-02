using APIMyMyStore.Entites;
using Microsoft.EntityFrameworkCore;

namespace APIMyMyStore.Contexts
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
    }
}
