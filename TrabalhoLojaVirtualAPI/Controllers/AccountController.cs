using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrabalhoLojaVirtualLibrary.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace TrabalhoLojaVirtualAPI.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        protected readonly AccountServices _accountService;
        public AccountController(AccountServices accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Registrar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar(Register registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _accountService.Register(registerUser);

            if (result > 0)
            {
                return Ok(await _accountService.JwtCreate(registerUser.Email));
            }

            return Problem("Falha ao registrar o usuário");
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(Login loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _accountService.Login(loginUser);

            if (result.Succeeded)
            {
                return Ok(await _accountService.JwtCreate(loginUser.Email));
            }

            return Problem("Usuario ou senha incorretos");
        }

    }
}
