using E_Commerce.API.Controllers;
using E_Commerce.Application.DTOs.Orders;
using E_Commerce.Application.Services.Class;
using E_Commerce.Application.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(c => { }, typeof(ApplicationServicesRegistration).Assembly);
            services.AddScoped<IProductSevcie, ProductService>();
            services.AddScoped<IBasketService, BasketSevice>();
            services.AddScoped<ICashService, CachService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
