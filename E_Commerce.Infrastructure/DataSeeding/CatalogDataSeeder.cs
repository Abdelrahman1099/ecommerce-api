using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Order;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext context, ILogger<StoreDbContext> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var PendingMigrations = await context.Database.GetPendingMigrationsAsync(ct);
                if (PendingMigrations.Any())
                {
                    await context.Database.MigrateAsync();
                }
                var SeedRootPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmpty<Product, int>(SeedRootPath, "products.json", ct);
                await SeedIfEmpty<ProductType, int>(SeedRootPath, "types.json", ct);
                await SeedIfEmpty<ProductBrand, int>(SeedRootPath, "brands.json", ct);
                await SeedIfEmpty<DeliveryMethod, int>(SeedRootPath, "delivery.json", ct);

                var count = await context.SaveChangesAsync(ct);
                if (count > 0)
                    logger.LogInformation($"{count} Rows Added");
                else
                    logger.LogInformation("Data Already Seeded");
            }
            catch (Exception ex)  
            {
                logger.LogError(ex.Message);
            }
        }

        private async Task SeedIfEmpty<TEntity, Tkey>(string rootPath, string fileName, CancellationToken ct = default) where TEntity : BaseEntity<Tkey>
        {
            if (await context.Set<TEntity>().AnyAsync()) 
            {
                var tableName = typeof(TEntity).Name;
                logger.LogWarning($"Table {tableName} Has Already Data");
                return;
            }
            var filePath = Path.Combine(rootPath, fileName);
            if (!File.Exists(filePath)) 
            {
                logger.LogWarning($"File {fileName} Not Exsist");
                return;
            }
            using var fileStream = File.OpenRead(filePath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(fileStream, options, ct);
            if(data is not null && data.Any())
                await context.Set<TEntity>().AddRangeAsync(data); 

        }
    }
}
