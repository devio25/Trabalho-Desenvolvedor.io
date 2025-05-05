using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TrabalhoLojaVirtualLibrary.Models;

namespace TrabalhoLojaVirtualLibrary.Data
{
    public class LojaVirtualDbContext : IdentityDbContext
    {
        public LojaVirtualDbContext(DbContextOptions<LojaVirtualDbContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Vendedor> Vendedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Produto>()
         .HasOne(p => p.Categoria)
         .WithMany(c => c.Produtos)
         .HasForeignKey(p => p.CategoriaId)
         .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
