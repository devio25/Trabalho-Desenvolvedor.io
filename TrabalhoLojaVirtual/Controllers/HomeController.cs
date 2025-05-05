using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrabalhoLojaVirtual.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace TrabalhoLojaVirtual.Controllers
{
    public class HomeController : Controller
    {
        protected readonly ProdutoServices _produtoService;

        public HomeController(ProdutoServices produtoService)
        {
            _produtoService = produtoService;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoService.ObterTodosAsync();
            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
