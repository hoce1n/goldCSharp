namespace Infrastructure.Payments.Zarinpal
{
    public class ZarinpalRequestResponse
    {
        public ZarinpalRequestData data { get; set; } = null!;
        public object[] errors { get; set; } = [];
    }

    public class ZarinpalRequestData
    {
        public int code { get; set; }
        public string message { get; set; }
        public string authority { get; set; } = null!;
    }

    public class ZarinpalVerifyResponse
    {
        public ZarinpalVerifyData data { get; set; } = null!;
    }

    public class ZarinpalVerifyData
    {
        public int code { get; set; }
        public long ref_id { get; set; }
    }

}
