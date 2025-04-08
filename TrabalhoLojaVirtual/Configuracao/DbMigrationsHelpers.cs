using Microsoft.EntityFrameworkCore;
using System;
using TrabalhoLojaVirtualLibrary.Data;
using TrabalhoLojaVirtualLibrary.Models;

namespace TrabalhoLojaVirtualMVC.Configuracao
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

            var context = scope.ServiceProvider.GetRequiredService<LojaVirtualDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
            {
                await context.Database.MigrateAsync();
                await EnsureSeedProducts(context);
            }
        }

        public static async Task EnsureSeedProducts(LojaVirtualDbContext context)
        {
            if (context.Categorias.Any())
                return;

            await context.Categorias.AddAsync(new Categoria
            {
                Descricao = "Brinquedos",
            });

            await context.SaveChangesAsync();
        }
    }


}
