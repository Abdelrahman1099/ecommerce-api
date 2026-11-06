using E_Commerce.Domain.Contracts.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class CachRepository(IConnectionMultiplexer connection) : Domain.Contracts.Repositories.ICachRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string key, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(key);

            if(value.IsNullOrEmpty) return null;
            return value;
        }

        public async Task SetAsync(string key, object value, TimeSpan? duration = null, CancellationToken ct = default)
        {
            var redisValue = JsonSerializer.Serialize(value);
            var result = await _database.StringSetAsync(key, redisValue, duration ?? TimeSpan.FromMinutes(5));
        }
    }
}
