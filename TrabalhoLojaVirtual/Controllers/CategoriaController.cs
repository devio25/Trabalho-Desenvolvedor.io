using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CategoriaController : Controller
    {
        private readonly CategoriaServices services;

        public CategoriaController(CategoriaServices services)
        {
            this.services = services;
        }



        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            return View(await services.ListaCategoria());
        }

        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var categoria = await services.DetalheCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                int result = await services.CriarCategoria(categoria);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(categoria);
            }
            return View(categoria);
        }

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
           
            var categoria = await services.DetalheCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int result = 0;

                result = await services.EditarCategoria(categoria);
                
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(categoria);
            }
            return View(categoria);
        }

        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int id)
        {            

            var categoria = await services.DetalheCategoria(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await services.DetalheCategoria(id);
            try
            {
                int result = 0;
                
                if (categoria != null)
                {

                    result = await services.DeletarCategoria(categoria);
                }

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(categoria);
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Não é possivel excluir uma categoria com um produto vinculado");
                return View(categoria);
            }
            

        }        
    }
}
