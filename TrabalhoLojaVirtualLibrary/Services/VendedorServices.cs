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
    public class VendedorServices
    {
        public LojaVirtualDbContext _dbContext;

        public VendedorServices(LojaVirtualDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Vendedor?> ObterPorLoginAsync(string loginId)
        {
            return await _dbContext.Vendedores
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.FKLogin == loginId);
        }
    }
}
