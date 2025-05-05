using Microsoft.AspNetCore.Http;
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
    public class ProdutoServices
    {
        protected readonly LojaVirtualDbContext _context;

        public ProdutoServices(LojaVirtualDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task SalvarImagemAsync(IFormFile imagem, string nomeArquivo)
        {
            if (imagem is null || imagem.Length == 0)
                return;

            var diretorioUploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");

            try
            {
                Directory.CreateDirectory(diretorioUploads); // Cria se já não existir

                var caminhoDestino = Path.Combine(diretorioUploads, nomeArquivo);

                await using var arquivoDestino = new FileStream(caminhoDestino, FileMode.Create);
                await imagem.CopyToAsync(arquivoDestino);
            }
            catch
            {
                throw new IOException("Erro ao tentar salvar a imagem no servidor.");
            }
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            var query = _context.Produtos
                .Include(produto => produto.Categoria)
                .Include(produto => produto.Vendedor);

            var resultado = await query.ToListAsync();
            return resultado;
        }

        public async Task<Produto?> ObterProdutoDoVendedor(int idProd, int idVend)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == idProd && p.VendedorId == idVend);
        }


        public async Task<IEnumerable<Produto>> ObterPorVendedorAsync(int vendedorId)
        {
            var produtos = _context.Produtos
                .Where(produto => produto.VendedorId == vendedorId)
                .Include(produto => produto.Categoria)
                .Include(produto => produto.Vendedor)
                .AsNoTracking();

            return await produtos.ToListAsync();
        }

        public async Task<Produto?> ObterDetalhesAsync(int produtoId)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == produtoId);

            return produto;
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoriaAsync(int categoriaId)
        {
            var produtos = _context.Produtos
                .Where(p => p.CategoriaId == categoriaId)
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .AsNoTracking();

            return await produtos.ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoriaDoVendedorAsync(int categoriaId, int idVend)
        {
            var produtos = _context.Produtos
                .Where(p => p.CategoriaId == categoriaId).Where(p => p.VendedorId == idVend)
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .AsNoTracking();

            return await produtos.ToListAsync();
        }

        public async Task<int> AdicionarAsync(Produto novoProduto)
        {
            await _context.Produtos.AddAsync(novoProduto);
            var linhasAfetadas = await _context.SaveChangesAsync();
            return linhasAfetadas;
        }

        public async Task<int> AtualizarAsync(Produto produtoAtualizado)
        {
            _context.Produtos.Update(produtoAtualizado);
            var linhasAfetadas = await _context.SaveChangesAsync();
            return linhasAfetadas;
        }

        public async Task<int> RemoverAsync(Produto produtoParaExcluir)
        {
            _context.Produtos.Remove(produtoParaExcluir);
            var linhasAfetadas = await _context.SaveChangesAsync();
            return linhasAfetadas;
        }

        public async Task ExcluirImagemAsync(string nomeImagem)
        {
            var caminhoImagem = Path.Combine("wwwroot", "imagens", nomeImagem);
            var caminhoCompleto = Path.GetFullPath(caminhoImagem);

            await Task.Run(() =>
            {
                if (File.Exists(caminhoCompleto))
                {
                    File.Delete(caminhoCompleto);
                }
                else
                {
                    throw new FileNotFoundException("Imagem não encontrada para exclusão.", nomeImagem);
                }
            });
        }

    }
}
