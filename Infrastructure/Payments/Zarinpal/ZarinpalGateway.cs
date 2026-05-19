using Application.Abstractions.Payments;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Infrastructure.Payments.Zarinpal
{
    public class ZarinpalGateway : IPaymentGateway
    {
        private readonly HttpClient _httpClient;
        private readonly ZarinpalOptions _options;
        private readonly ILogger<ZarinpalGateway> _logger;

        public ZarinpalGateway(
            HttpClient httpClient, 
            IOptions<ZarinpalOptions> options,
            ILogger<ZarinpalGateway> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        private string BaseUrl =>
            _options.UseSandbox
            ? "https://sandbox.zarinpal.com"
            : "https://payment.zarinpal.com";

        public async Task<PaymentGatewayResult> CreatePayment(
            Guid paymentId,
            decimal amount,
            CancellationToken cancellationToken)
        {
            var request = new
            {
                merchant_id = _options.MerchantId,
                amount = (int)amount,
                callback_url = $"{_options.CallbackUrl}?paymentId={paymentId}",
                description = "Gold purchase payment"
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/pg/v4/payment/request.json",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ZarinpalRequestResponse>(cancellationToken: cancellationToken);

            if (result?.data?.code != 100)
            {
                _logger.LogError("Zarinpal request failed");
                throw new Exception("Payment request failed");
            }

            return new PaymentGatewayResult
            {
                Authority = result.data.authority,
                PaymentUrl = $"{BaseUrl}/pg/StartPay/{result.data.authority}"
            };
        }

        public async Task<PaymentVerifyResult> VerifyPayment(
            string authority,
            decimal amount,
            CancellationToken cancellationToken)
        {
            var request = new
            {
                merchant_id = _options.MerchantId,
                amount = (int)amount,
                authority = authority
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/pg/v4/payment/verify.json",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ZarinpalVerifyResponse>(
                cancellationToken: cancellationToken);

            if (result?.data?.code == 100 || result?.data?.code == 101)
            {
                return new PaymentVerifyResult
                {
                    IsSuccessful = true,
                    RefId = result.data.ref_id.ToString()
                };
            }

            return new PaymentVerifyResult
            {
                IsSuccessful = false,
                Message = "Payment verification failed"
            };
        }

    }
}
