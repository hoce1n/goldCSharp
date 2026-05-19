namespace Infrastructure.Payments.Zarinpal
{
    public class ZarinpalOptions
    {
        public string MerchantId { get; set; } = null!;
        public string CallbackUrl { get; set; } = null!;
        public bool UseSandbox { get; set; }
    }
}
