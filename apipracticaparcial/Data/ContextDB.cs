using apipracticaparcial.Models;
using Microsoft.EntityFrameworkCore;

namespace apipracticaparcial.Data
{
    public class ContextDB:DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }








    }
}
