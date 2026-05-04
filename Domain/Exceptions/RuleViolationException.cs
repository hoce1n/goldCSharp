namespace Domain.Exceptions
{
    public class RuleViolationException : DomainException
    {
        public RuleViolationException(string ruleName)
            : base("Domain.RuleViolation", $"Rule violated: {ruleName}")
        {
        }
    }
}