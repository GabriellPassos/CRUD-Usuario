using CrudUsuarios.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuarios.Data
{
   public class AppDbContext : DbContext
    {
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
