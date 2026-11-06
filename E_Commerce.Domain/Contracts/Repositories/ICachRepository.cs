using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts.Repositories
{
    public interface ICachRepository
    {
        Task<string?> GetAsync(string key, CancellationToken ct = default);
        Task SetAsync(string key, object value, TimeSpan? duration = null, CancellationToken ct =default);
    }
}
