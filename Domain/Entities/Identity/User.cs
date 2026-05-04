using Domain.Common;
using Domain.Enums;
using Domain.Events.User;
using Domain.Exceptions.User;
using Domain.Excetions.Auth;
using Domain.ValueObjects;

namespace Domain.Entities.Identity
{
    public class User : BaseEntity
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public NationalCode? NationalCode { get; private set; }
        public DateTime? BirthDate { get; private set; }
        public Email? Email { get; private set; }
        public bool IsPhoneVerified { get; private set; }

        public UserStatus Status { get; private set; }
        public VerificationLevel VerificationLevel { get; private set; }


        private readonly List<UserRole> _roles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _roles;


        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        private User() {}

        public User(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
            Status = UserStatus.Active;
            VerificationLevel = VerificationLevel.Unverified;

            AddDomainEvent(new UserRegisteredEvent(Id));
        }

        public void SetProfile(
            string firstName, 
            string lastName, 
            DateTime bithDate)
        {
            EnsureIsActive();

            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidFirstNameException();

            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidLastNameException();


            if (CalculateAge(bithDate) < 18)
                throw new UserMustBeAdultException();

            FirstName = firstName;
            LastName = lastName;
            BirthDate = bithDate;

            AddDomainEvent(new UserProfileCompletedEvent(Id));
        }
        public void SetEmail(Email email)
        {
            Email = email;
        }

        public void SetNationalCode(NationalCode code)
        {
            if (NationalCode != null)
                throw new NationalCodeAlreadySetException();

            NationalCode = code;
            VerificationLevel = VerificationLevel.Basic;

            
            AddDomainEvent(new NationalCodeVerifiedEvent(Id, NationalCode));
        }


        public void VerifyPhone()
        {
            IsPhoneVerified = true;
            SetUpdated();
        }

        public bool HasRole(Guid roleId)
        {
            return _roles.Any(r => r.RoleId == roleId);
        }

        public void AddRole(Role role)
        {
            if (_roles.Any(r => r.RoleId == role.Id))
            return;

            _roles.Add(new UserRole(Id, role.Id));
        }
        public void RemoveRole(Role role)
        {
            var userRole = _roles.FirstOrDefault(r => r.RoleId == role.Id);

            if (userRole != null)
                _roles.Remove(userRole);
        }

        public void EnsureIsActive()
        {
            if (Status == UserStatus.Blocked)
                throw new UserBlockedException();
        }
        public void EnsureCanTrade()
        {
            if (VerificationLevel < VerificationLevel.Basic)
                throw new UserNotVerifiedException();
        }

        public void Block()
        {
            if (Status == UserStatus.Blocked)
                return;

            Status = UserStatus.Blocked;
            AddDomainEvent(new UserBlockedEvent(Id));
        }

        public void Activate()
        {
            Status = UserStatus.Active;
            AddDomainEvent(new UserActivatedEvent(Id));
        }

        public RefreshToken? GetActiveRefreshToken()
        {
            return _refreshTokens.LastOrDefault(t => t.IsActive);
        }
        public void AddRefreshToken(RefreshToken token)
        {
            EnsureIsActive();
            if (token == null)
                throw new InvalidRefreshTokenException();

            _refreshTokens.Add(token);
        }
        public void RevokeRefreshToken(Guid tokenId)
        {
            var token = _refreshTokens.FirstOrDefault(t => t.Id == tokenId);
            token?.Revoke();
        }
        public void ReplaceRefreshToken(Guid oldTokenId, RefreshToken newToken)
        {
            EnsureIsActive();

            var oldToken = _refreshTokens.FirstOrDefault(t => t.Id == oldTokenId);

            if (oldToken is null)
                throw new RefreshTokenNotFoundException();

            if (!oldToken.IsActive)
                throw new RefreshTokenIsRevokedException();

            oldToken.SetReplacedBy(newToken.Id);
            oldToken.Revoke();

            _refreshTokens.Add(newToken);
        }


        public string FullName => $"{FirstName} {LastName}".Trim();

        public bool IsProfileCompleted =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            BirthDate.HasValue &&
            NationalCode != null;

        private static int CalculateAge(DateTime bithDate)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - bithDate.Year;

            if (bithDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}