using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrabalhoLojaVirtualLibrary.Data;
using TrabalhoLojaVirtualLibrary.Models;

namespace TrabalhoLojaVirtualLibrary.Services
{
    public class AccountServices
    {
        protected readonly SignInManager<IdentityUser> _signManager;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly JwtSettings _jwtSettings;
        protected readonly LojaVirtualDbContext _dbContext;


        public AccountServices(SignInManager<IdentityUser> signManager, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings, LojaVirtualDbContext dbContext)
        {
            _signManager = signManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _dbContext = dbContext;
        }

        public async Task<string> JwtCreate(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            });

            return tokenHandler.WriteToken(token);
        }

        public async Task<SignInResult> Login(Login login)
        {
            var result = await _signManager.PasswordSignInAsync(login.Email, login.Password, false, true);
            return result;
        }

        public async Task<int> Register(Register register)
        {
            int resultado = 0;
            IdentityUser user = new IdentityUser
            {
                UserName = register.Email,
                Email = register.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                Vendedor vendedor = new Vendedor
                {
                    DataCadastro = DateTime.Now,
                    Nome = register.Nome,
                    FKLogin = user.Id,

                };
                await _dbContext.Vendedores.AddAsync(vendedor);

                resultado = await _dbContext.SaveChangesAsync();
            }

            return resultado;
        }

        public async Task Logout()
        {
            await _signManager.SignOutAsync();
        }

    }

}
