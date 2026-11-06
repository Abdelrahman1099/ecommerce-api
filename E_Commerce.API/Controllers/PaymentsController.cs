using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.API.Controllers
{

    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentGatewaySettings _options;

        public PaymentsController(IPaymentService paymentService, IOptions<PaymentGatewaySettings> options)
        {
            _paymentService = paymentService;
            _options = options.Value;
        }

        [Authorize]
        [HttpPost("{basketId}")]

        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken ct)
        {
            return ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct));
        }


        [HttpPost("webhook")]

        public async Task<IActionResult> StripeWebhook()
        {
            var requestJson = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(requestJson,
                    Request.Headers["Stripe-Signature"], _options.WebhookSecret);

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:

                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntent is not null)
                            await _paymentService.PaymentSuccessed(paymentIntent.Id);

                        break;

                    case EventTypes.PaymentIntentPaymentFailed:

                        var paymentIntentFailed = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntentFailed is not null)
                            await _paymentService.PaymentFailed(paymentIntentFailed.Id);

                        break;

                    default:
                        break;

                }

                return Ok();

            }
            catch (StripeException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

