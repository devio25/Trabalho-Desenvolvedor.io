using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TrabalhoLojaVirtualLibrary.Data;
using TrabalhoLojaVirtualLibrary.Models;
using TrabalhoLojaVirtualLibrary.Services;

namespace TrabalhoLojaVirtualLibrary.Configuracao
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {

        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateAsyncScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var account = scope.ServiceProvider.GetRequiredService<AccountServices>();

            var context = scope.ServiceProvider.GetRequiredService<LojaVirtualDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
            {
                await context.Database.MigrateAsync();
                await EnsureSeedProducts(context, account);
            }
        }

        public static async Task EnsureSeedProducts(LojaVirtualDbContext context, AccountServices service)
        {
            if (context.Categorias.Any())
                return;

            await context.Categorias.AddAsync(new Categoria
            {
                Descricao = "Brinquedos",
            });

            await context.Categorias.AddAsync(new Categoria
            {
                Descricao = "Móveis",
            });

            await context.Categorias.AddAsync(new Categoria
            {
                Descricao = "Eletro",
            });

            await context.SaveChangesAsync();

            if (context.Users.Any())
                return;

            Register register = new Register();
            register.Email = "alberto@gmail.com";
            register.Nome = "Alberto";
            register.Password = "Teste@123";
            register.ConfirmPassword = "Teste@123";

            await service.Register(register);

            register = new Register();
            register.Email = "patricia@gmail.com";
            register.Nome = "Patricia";
            register.Password = "Teste@123";
            register.ConfirmPassword = "Teste@123";

            await service.Register(register);

            register = new Register();
            register.Email = "leandro@gmail.com";
            register.Nome = "Leandro";
            register.Password = "Teste@123";
            register.ConfirmPassword = "Teste@123";

            await service.Register(register);


            if (context.Produtos.Any())
                return;

            List<Produto> produtos = new List<Produto>();
            produtos.Add(new Produto
            {

                Descricao = "Lego Creator",
                Estoque = 5,
                Imagem = "brinquedo_01.jpg",
                VendedorId = 1,
                CategoriaId = 1,
                Valor = 500.99m
            });

            produtos.Add(new Produto
            {
                Descricao = "Cadeira",
                Estoque = 12,
                Imagem = "cadeira_01.jpg",
                VendedorId = 1,
                CategoriaId = 2,
                Valor = 630.00m
            });

            produtos.Add(new Produto
            {
                Descricao = "Estante",
                Estoque = 30,
                Imagem = "estante_01.jpg",
                VendedorId = 1,
                CategoriaId = 2,
                Valor = 850.00m
            });

            produtos.Add(new Produto
            {
                Descricao = "Mesa",
                Estoque = 90,
                Imagem = "mesa.jpg",
                VendedorId = 2,
                CategoriaId = 2,
                Valor = 12.00m
            });

            produtos.Add(new Produto
            {
                Descricao = "Pipoqueira",
                Estoque = 10,
                Imagem = "pipoqueira_01.jpg",
                VendedorId = 3,
                CategoriaId = 3,
                Valor = 230.90m
            });

            await context.Produtos.AddRangeAsync(produtos);
            await context.SaveChangesAsync();


        }
    }


}
