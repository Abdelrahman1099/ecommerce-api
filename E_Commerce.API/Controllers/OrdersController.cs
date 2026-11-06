using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Entities.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.API.Controllers
{
    public class OrdersController(IOrderService orderService) : ApiBaseController
    {
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<OrderToReturn>> CreateOrder(OrderDto dto, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await orderService.CreateOrderAsync(dto, email, ct);
            return ToActionResult(result);
        }


        [HttpGet]
        [Authorize]

        public async Task<ActionResult<IReadOnlyList<OrderToReturn>>> GetOrders(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await orderService.GetOrdersForSpecificUserAsync(email, ct);

            return ToActionResult(result);
        }



        [HttpGet("{id:guid}")]
        [Authorize]

        public async Task<ActionResult<OrderToReturn>> GetOrderById(Guid id, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await orderService.GetOrdersByIdForSpecificUserAsync(id, email, ct);

            return ToActionResult(result);
        }


        [HttpGet("DeliveryMethods")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods(CancellationToken ct = default)
        {
            var result = await orderService.GetAllDeliveryMethodAsync(ct);
            return ToActionResult(result);
        }

    }
}
