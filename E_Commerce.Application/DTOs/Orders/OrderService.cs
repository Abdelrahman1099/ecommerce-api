using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Order;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Orders
{
    internal class OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<Result<OrderToReturn>> CreateOrderAsync(OrderDto dto, string email, CancellationToken ct = default)
        {
            var orderAdress = mapper.Map<OrderAdress>(dto.ShipToAddress);

            var basket = await basketRepository.GetBasketAsync(dto.BasketId, ct);
            if (basket is null)
                return Result<OrderToReturn>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id {dto.BasketId} Is Not Found"));

            var exOrder = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(basket.PaymentIntentId));

            if (exOrder != null)
                unitOfWork.GetRepository<Order, Guid>().Delete(exOrder);

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Result<OrderToReturn>.Fail(Error.NotFound("Product.NotFound", $"Product With Id {item.Id} Is Not Found"));

                if(basket.Items.Count == 0)
                    return Result<OrderToReturn>.Fail(Error.Validation("Basket Is Empty", $"Can Not Create Order With Basket Id {basket.Id}"));


                var productItem = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                if(product.Price != item.Price) item.Price = product.Price;

                var orderItem = new OrderItem(productItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(dto.DeliveryMethodId, ct);
            if(deliveryMethod is null)
                return Result<OrderToReturn>.Fail(Error.NotFound("DeliveryMethod.NotFound", $"DeliveryMethod With Id {dto.DeliveryMethodId} Is Not Found"));

            var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);

            var order = new Order(email, orderAdress, orderItems, deliveryMethod, subTotal, basket.PaymentIntentId);

            unitOfWork.GetRepository<Order, Guid>().Add(order); 
            var count = await unitOfWork.SaveChangesAsync(ct);

            if (count <= 0)
                return Result<OrderToReturn>.Fail(Error.Failure("Create.Order.Failure", "Can Not Create This Order"));

            await basketRepository.DeleteBasketAsync(dto.BasketId, ct);

            var orderToReturn = mapper.Map<OrderToReturn>(order);
            return Result<OrderToReturn>.Ok(orderToReturn);

            throw new NotImplementedException();
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodAsync(CancellationToken ct = default)
        {
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);

            if(deliveryMethod.Any())
            {
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethod));
            }

            return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(Error.NotFound("DeliveryMethods.NotFound", "Not Found Any DeliveryMethods"));
        }

        public async Task<Result<OrderToReturn>> GetOrdersByIdForSpecificUserAsync(Guid orderId, string email, CancellationToken ct = default)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecifications(orderId, email), ct);

            if (order is null)
                return Result<OrderToReturn>.Fail(Error.NotFound("Order.NotFound", "Order Is Not Found"));

            return Result<OrderToReturn>.Ok(mapper.Map<OrderToReturn>(order));
        }

        public async Task<Result<IReadOnlyList<OrderToReturn>>> GetOrdersForSpecificUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecifications(email), ct);

            if(orders.Any())
            {
                return Result<IReadOnlyList<OrderToReturn>>.Ok(mapper.Map<IReadOnlyList<OrderToReturn>>(orders));
            }

            return Result<IReadOnlyList<OrderToReturn>>.Fail(Error.NotFound("Orders.NotFound", $"User With Email {email} Don't Have Any Order"));
        }
    }
}
