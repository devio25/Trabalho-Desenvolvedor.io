using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoLojaVirtualLibrary.Data;
using TrabalhoLojaVirtualLibrary.Models;

namespace TrabalhoLojaVirtualLibrary.Services
{
    public class CategoriaServices
    {
        public LojaVirtualDbContext Context;

        public CategoriaServices(LojaVirtualDbContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<Categoria>> ListaCategoria()
        {
            return await Context.Categorias.ToListAsync();
        }

        

        public async Task<Categoria> DetalheCategoria(int id)
        {
            return await Context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> CriarCategoria(Categoria categoria)
        {
            Context.Add(categoria);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> EditarCategoria(Categoria categoria)
        {
            Context.Update(categoria);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> DeletarCategoria(Categoria categoria)
        {
            Context.Remove(categoria);
            return await Context.SaveChangesAsync();
        }
    }
}
