using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(string userId, string email, string userName, IReadOnlyList<string> roles);
    }
}
