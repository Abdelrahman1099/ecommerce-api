using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Class
{
    public class BasketSevice(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto dto, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var basket = mapper.Map<CustomerBasket>(dto);
            var result = await basketRepository.CreateOrUpdateBasketAsync(basket, timeToLive, ct);

            if (result is null)
                return Result<BasketDto>.Fail(Error.Failure("Basket.CreateOrUpdate.Failure", "Failed to create or update the basket"));

            return Result<BasketDto>.Ok(dto);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string BasketId, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(BasketId, ct);
            if (basket is null)
                return Result<bool>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id {BasketId} is not Found"));

            var result = await basketRepository.DeleteBasketAsync(BasketId, ct);

            return result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("Basket.Delete.Failure", $"Can Not Delete This Basket With Id {BasketId}"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string BasketId, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(BasketId, ct);

            if (basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket.NotFound", $"Basket With Id {BasketId} is not Found"));

            var dto = mapper.Map<BasketDto>(basket);
            return Result<BasketDto>.Ok(dto);
        }
    }
}
