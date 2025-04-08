using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoLojaVirtualLibrary.Models;

namespace TrabalhoLojaVirtualLibrary.Data
{
    public class LojaVirtualDbContext : DbContext
    {
        public LojaVirtualDbContext(DbContextOptions<LojaVirtualDbContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }
    }
}
