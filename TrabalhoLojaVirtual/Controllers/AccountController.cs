using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrabalhoLojaVirtualLibrary.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace TrabalhoLojaVirtualMVC.Controllers
{
    public class AccountController : Controller
    {

        protected readonly AccountServices _accountService;

        public AccountController(AccountServices accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var result = await _accountService.Login(login);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Categoria");
            }

            ModelState.AddModelError("", "Usuário ou senha inválidos");
            return View(login);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (!ModelState.IsValid)
                return View(register);

            var result = await _accountService.Register(register);

            if (result > 0)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Erro ao registrar usuário");
            return View(register);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Login");
        }
    }

}
