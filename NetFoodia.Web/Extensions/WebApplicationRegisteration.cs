using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Contracts;
using NetFoodia.Persistence.Data.DbContexts;


namespace NetFoodia.Web.Extensions
{
    public static class WebApplicationRegisteration
    {
        //public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        //{
        //    await using var Scope = app.Services.CreateAsyncScope();

        //    var dbContextService = Scope.ServiceProvider.GetRequiredService<NetFoodiaDbContext>();

        //    var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
        //    if (PendingMigrations.Any())
        //        await dbContextService.Database.MigrateAsync();

        //    return app;
        //}

        //public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        //{
        //    await using var Scope = app.Services.CreateAsyncScope();
        //    var DataInatializerService = Scope.ServiceProvider.GetRequiredKeyedService<IDataInatializer>("Default");
        //    await DataInatializerService.InatializeAsync();

        //    return app;
        //}
        public static async Task<WebApplication> MigrateIdentityDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();

            var dbContextService = Scope.ServiceProvider.GetRequiredService<NetFoodiaDbContext>();

            var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
                await dbContextService.Database.MigrateAsync();

            return app;
        }
        public static async Task<WebApplication> SeedIdentityDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var DataInatializerService = Scope.ServiceProvider.GetRequiredKeyedService<IDataInatializer>("Identity");
            await DataInatializerService.InatializeAsync();

            return app;
        }
    }
}
