using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.API.Controllers
{
    public class BasketsController(IBasketService basketService) : ApiBaseController
    {
        [HttpGet("{id}")]

        public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken ct = default)
        {
            var result = await basketService.GetBasketAsync(id, ct);
            return ToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBaskets(BasketDto dto, CancellationToken ct = default)
        {
            var result = await basketService.CreateOrUpdateBasketAsync(dto,null, ct);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct = default)
        {
            var result = await basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
    }
}
