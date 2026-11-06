using E_Commerce.Application.Common;
using E_Commerce.Application.Services.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Payment
{
    internal class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentIntentService _paymentIntentService = new();
        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options) 
        {
            StripeConfiguration.ApiKey = options.Value.Secretkey;
        }
        public async Task<PaymentIntentResult> CreatePaymentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions()
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                PaymentMethodTypes = ["card"]
            };
            var intent = await _paymentIntentService.CreateAsync(options, cancellationToken: ct);
            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentAsync(decimal amount, string paymentId, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = (long)amount
            };
            var intent = await _paymentIntentService.UpdateAsync(paymentId, options, cancellationToken: ct);
            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }
    }
}
