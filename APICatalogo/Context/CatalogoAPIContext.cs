using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context
{
    public class CatalogoAPIContext : DbContext
    {
        public CatalogoAPIContext(DbContextOptions<CatalogoAPIContext> options) : base(options)
        {

        }

        public DbSet<Category>? Categorys { get; set; }
        public DbSet<Product>? Products { get; set; }
    }
}
