using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabalhoLojaVirtualLibrary.Data;
using TrabalhoLojaVirtualLibrary.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace TrabalhoLojaVirtualMVC.Controllers
{
    [Authorize]
    [Route("produto")]
    public class ProdutoController : Controller
    {
        private readonly ProdutoServices _produtoServices;
        private readonly CategoriaServices _categoriaServices;
        private readonly VendedorServices _vendedorServices;

        public ProdutoController(ProdutoServices produtoServices, CategoriaServices categoriaServices, VendedorServices vendedorServices)
        {
            _produtoServices = produtoServices;
            _categoriaServices = categoriaServices;
            _vendedorServices = vendedorServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var vendedor = await _vendedorServices.ObterPorLoginAsync(userId);

            if (vendedor is null)
                return NotFound("Vendedor não encontrado.");

            var produtos = await _produtoServices.ObterPorVendedorAsync(vendedor.Id);

            return View(produtos);
        }

        [HttpGet("Detalhes")]
        public async Task<IActionResult> Details(int id)
        {
            var produto = await _produtoServices.ObterDetalhesAsync(id);

            if (produto is null)
                return NotFound();

            return View(produto);
        }

        [HttpGet("Criar")]
        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriaServices.ListaCategoria();
            ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Descricao");

            return View();
        }

        [HttpPost("Criar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,Estoque,Upload,CategoriaId")] Produto produto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(await _categoriaServices.ListaCategoria(), "Id", "Descricao", produto.CategoriaId);
                return View(produto);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var vendedor = await _vendedorServices.ObterPorLoginAsync(userId);

                if (vendedor is null)
                    return View(produto);

                //produto.Vendedor = vendedor;
                produto.VendedorId = vendedor.Id;

                var nomeImagem = Guid.NewGuid() + Path.GetExtension(produto.Upload.FileName);
                await _produtoServices.SalvarImagemAsync(produto.Upload, nomeImagem);

                produto.Imagem = nomeImagem;

                var resultado = await _produtoServices.AdicionarAsync(produto);

                if (resultado > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(produto);
        }

        [HttpGet("Editar")]
        public async Task<IActionResult> Edit(int id)
        {
            var produto = await _produtoServices.ObterDetalhesAsync(id);
            if (produto is null)
                return NotFound();

            ViewData["CategoriaId"] = new SelectList(await _categoriaServices.ListaCategoria(), "Id", "Descricao");
            ViewData["VendedorId"] = produto.VendedorId;

            return View(produto);
        }

        [HttpPost("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,Estoque,Upload,CategoriaId,VendedorId")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                var produtoExistente = await _produtoServices.ObterDetalhesAsync(id);

                if (produtoExistente is not null)
                {
                    await _produtoServices.ExcluirImagemAsync(produtoExistente.Imagem);
                }

                var nomeImagem = Guid.NewGuid() + Path.GetExtension(produto.Upload.FileName);
                produto.Imagem = nomeImagem;
                await _produtoServices.SalvarImagemAsync(produto.Upload, nomeImagem);

                var resultado = await _produtoServices.AtualizarAsync(produto);

                if (resultado > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoriaId"] = new SelectList(await _categoriaServices.ListaCategoria(), "Id", "Descricao", produto.CategoriaId);
            return View(produto);
        }

        [HttpGet("Deletar")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _produtoServices.ObterDetalhesAsync(id);
            if (produto is null)
                return NotFound();

            return View(produto);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmado(int id)
        {

            var produto = await _produtoServices.ObterDetalhesAsync(id);
            if (produto is null)
            {
                return View();
            }

            var resultado = await _produtoServices.RemoverAsync(produto);

            if (resultado > 0)
            {
                await _produtoServices.ExcluirImagemAsync(produto.Imagem);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
