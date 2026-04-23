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
            public const string OTPAlreadyUsed = "OTP.AlreadyUsed";
            public const string TooManyRequests = "OTP.TooManyRequests";
        }

        public static class RefreshToken
        {
            public const string Invalid = "RefreshToken.Invalid";
            public const string Expired = "RefreshToken.Expired";
            public const string Revoked = "RefreshToken.Revoked";
            public const string NotFound = "RefreshToken.NotFound";

        }

    }
}
