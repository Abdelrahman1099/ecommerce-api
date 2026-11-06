using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;

namespace E_Commerce.API.Controllers
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default);
        Task PaymentSuccessed(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);
    }
}
