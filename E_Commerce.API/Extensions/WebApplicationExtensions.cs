using E_Commerce.Domain.Contracts;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedMigratioDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("catalog");
            var IdentityDataSeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await dataSeeder.SeedDataAsync();
            await IdentityDataSeeder.SeedDataAsync();
            return app;
        }
    }
}
