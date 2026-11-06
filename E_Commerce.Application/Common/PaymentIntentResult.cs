using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public sealed class PaymentIntentResult
    {
        public PaymentIntentResult(string paymentIIntentId, string clientSecret)
        {
            PaymentIIntentId = paymentIIntentId;
            ClientSecret = clientSecret;
        }

        public string PaymentIIntentId { get; set; } = default;
        public string ClientSecret { get; set; } = default;
    }
}
