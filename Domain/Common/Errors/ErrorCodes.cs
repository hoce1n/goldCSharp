namespace Domain.Common.Errors
{
    public static class ErrorCodes
    {
        public static class General
        {
            public const string Unexpected = "General.Unexpected";
            public const string Validation = "General.Validation";
            public const string NotFound = "General.NotFound";
        }

        public static class Auth
        {
            public const string Forbidden = "Auth.Forbidden";
            public const string Unauthorized = "Auth.Unauthorized";
            public const string RuleViolation = "Auth.RuleViolation";
        }

        public static class User
        {
            public const string InvalidEmail = "User.InvalidEmail";
            public const string InvalidFirstName = "User.InvalidFirstName";
            public const string InvalidLastName = "User.InvalidLastName";
            public const string InvalidNationalCode = "User.InvalidNationalCode";
            public const string InvalidPhoneNumber = "User.InvalidPhoneNumber";
            public const string PhoneNumberRequired = "User.PhoneNubmerRequired";
            public const string NationalCodeAlreadySet = "User.NationalCodeAlreadySet";
            public const string Unauthorized = "User.Unauthorized";
            public const string Blocked = "User.Blocked";
            public const string NotFound = "User.NotFound";
            public const string NotVerified = "User.NotVerified";
            public const string MustBeAdult = "User.MustBeAdult";
        }

        public static class OTP
        {
            public const string Invalid = "OTP.Invalid";
            public const string Expired = "OTP.Expired";
            public const string NotFound = "OTP.NotFound";
            public const string LockedOut = "OTP.LockedOut";
            public const string OTPAlreadyUsed = "OTP.AlreadyUsed";
            public const string TooManyRequests = "OTP.TooManyRequests";
        }

        public static class RefreshToken
        {
            public const string Invalid = "RefreshToken.Invalid";
            public const string Expired = "RefreshToken.Expired";
            public const string Revoked = "RefreshToken.Revoked";
            public const string NotFound = "RefreshToken.NotFound";
            public const string Unauthorized = "RefreshToken.Unauthorized";

        }

        public static class Coin
        {
            public const string InvalidCoinName = "Coin.InvalidCoinName";
            public const string InvalidWeight = "Coin.InvalidWeight";
            public const string InvalidKarat = "Coin.InvalidKarat";
            public const string InvalidMintingFee = "Coin.InvalidMintingFee";
            public const string InvalidStock = "Coin.InvalidStock";
            public const string InsufficientStock = "Coin.InsufficientStock";
            public const string NotFound = "Coin.NotFound";
            public const string DuplicateName = "Coin.DuplicateName";
            public const string IdNotFound = "Coin.IdNotFound";
        }

        public static class Quote
        {
            public const string Expired = "Quote.Expired";
            public const string InvalidAmount = "Quote.InvalidAmount";
            public const string InvalidProduct = "Quote.InvalidProduct";
            public const string IsNotActive = "Quote.IsNotActive";
            public const string QuoteAlreadyConfirmed = "Quote.QuoteAlreadyConfirmed";
            public const string NotFound = "Quote.NotFound";
            public const string IdempotencyRequired = "Quote.IdempotencyRequired";
        }

        public static class Order
        {
            public const string CanNotBeCompleted = "Order.CanNotBeCompleted";
            public const string OnlyActiveOrdersCanBeCanceled = "Order.OnlyActiveOrdersCanBeCanceled";
            public const string OrderCannotBeFailed = "Order.OrderCannotBeFailed";
        }

        public static class Wallet
        {
            public const string NotFound = "Wallet.NotFound";
            public const string InsufficientBalance = "Wallet.InsufficientBalance";
            public const string InvalidWalletAmount = "Wallet. InvalidWalletAmountException";
        }

        public static class Payment
        {
            public const string NotFound = "Payment.NOTFOUND";
            public const string InvalidPaymentAmount = "Payment.InvalidPaymentAmount";
            public const string InvalidPaymentState = "Payment.InvalidPaymentState";
            public const string AlreadyVerified = "Payment.AlreadyVerified";
            public const string VerificationFailed = "Payment.VerificationFailed";
        }
    }
}
