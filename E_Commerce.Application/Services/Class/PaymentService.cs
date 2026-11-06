using AutoMapper;
using E_Commerce.API.Controllers;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Order;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Class
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySettings _paymentGatewaySettings;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,
            IPaymentGateway paymentGateway, IOptions<PaymentGatewaySettings> options, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _mapper = mapper;
            _paymentGatewaySettings = options.Value;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {

            #region 1. Get Basket [Validate]

            var basket = await _basketRepository.GetBasketAsync(basketId, ct);

            if (basket == null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket Not Found", $"Basket With Id {basketId} Not Found"));

            if (basket.Items.Count == 0)
                return Result<BasketDto>.Fail(Error.Validation("Basket Is Empty"));

            #endregion

            #region 2. Get DeliveryMethod -> cost

            if (!basket.DeliveryMethodId.HasValue)
                return Result<BasketDto>.Fail(Error.Validation("Delivery Method Id Is Required"));

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod == null)
                return Result<BasketDto>.Fail(Error.NotFound("Delivery Method Not Found"));

            basket.ShippingPrice = deliveryMethod.Cost;

            #endregion

            #region 3. Product Prices

            var productsIds = basket.Items.Select(x => x.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>().
                GetAllAsync(new ProductsWithIdsSpecifications(productsIds), ct)).ToDictionary(x => x.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<BasketDto>.Fail(Error.NotFound("Product Not Found"));
                item.Price = product.Price;
            }

            #endregion

            var suntotal = basket.Items.Sum(i => i.Price * i.Quantity);
            var amount = (long) ((suntotal + deliveryMethod.Cost) * 100m);

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var result = await _paymentGateway.CreatePaymentAsync(amount, _paymentGatewaySettings.DefaultCurrency, ct);
                basket.PaymentIntentId = result.PaymentIIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else
            {
                await _paymentGateway.UpdatePaymentAsync(amount, basket.PaymentIntentId, ct);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket, ct: ct);

            return Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basket));
        }

        public async Task PaymentFailed(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(paymentIntentId));

            if(order == null)
                return;
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task PaymentSuccessed(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new PaymentIntentSpec(paymentIntentId));

            if (order == null)
                return;
            order.Status = OrderStatus.PaymentReceived;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

