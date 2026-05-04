namespace Application.Features.Auth.Query.GetCurrentUser
{
    public class GetCurrentUserResponse
    {
       public Guid Id { get; init; }
        public string PhoneNumber { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? NationalCode { get; init; }
        public string? Birthdate { get; init; }
        public string? Email { get; init; }
        public string VerificationLevel { get; init; }
        public bool IsProfileCompleted { get; init; }
    }
}
