using Application.Abstractions.Payments;

namespace Infrastructure.Payments.Mock
{
    public class MockPaymentGateway : IPaymentGateway
    {
        private readonly Dictionary<Guid, MockPaymentSession> _sessions = new();

        public Task<PaymentGatewayResult> CreatePayment(
            Guid paymentId,
            decimal amount,
            CancellationToken cancellationToken)
        {
            // تولید Authority یکتا
            var authority = $"MOCK_AUTH_{paymentId:N}_{DateTime.UtcNow.Ticks}";

            // ذخیره جلسه پرداخت
            _sessions[paymentId] = new MockPaymentSession
            {
                PaymentId = paymentId,
                Amount = amount,
                Authority = authority,
                CreatedAt = DateTime.UtcNow
            };

            // ساخت لینک شبیه‌سازی شده درگاه
            var paymentUrl = $"http://localhost:3000/mock-payment?authority={authority}&paymentId={paymentId}&amount={amount}";

            return Task.FromResult(new PaymentGatewayResult
            {
                Authority = authority,
                PaymentUrl = paymentUrl
            });
        }

        public Task<PaymentVerifyResult> VerifyPayment(
            string authority,
            decimal amount,
            CancellationToken cancellationToken)
        {
            // پیدا کردن جلسه پرداخت بر اساس Authority
            var session = _sessions.Values
                .FirstOrDefault(s => s.Authority == authority);

            if (session is null)
            {
                return Task.FromResult(new PaymentVerifyResult
                {
                    IsSuccessful = false,
                    RefId = null,
                    Message = "جلسه پرداخت یافت نشد."
                });
            }

            // بررسی انقضا (مثلاً 10 دقیقه)
            if (session.CreatedAt < DateTime.UtcNow.AddMinutes(-10))
            {
                return Task.FromResult(new PaymentVerifyResult
                {
                    IsSuccessful = false,
                    RefId = null,
                    Message = "زمان جلسه پرداخت به اتمام رسیده است."
                });
            }

            // تأیید پرداخت موفق
            session.IsVerified = true;

            return Task.FromResult(new PaymentVerifyResult
            {
                IsSuccessful = true,
                RefId = $"MOCK_REF_{session.PaymentId:N}_{DateTime.UtcNow.Ticks}",
                Message = "پرداخت با موفقیت انجام شد."
            });
        }

        // کلاس داخلی برای نگهداری جلسات پرداخت
        private class MockPaymentSession
        {
            public Guid PaymentId { get; set; }
            public decimal Amount { get; set; }
            public string Authority { get; set; } = null!;
            public DateTime CreatedAt { get; set; }
            public bool IsVerified { get; set; }
        }
    }
}