using Ardalis.GuardClauses;

namespace LineTenTest.Api.Extensions
{
    public static class GuardClauseExtensions
    {
        public static string InvalidEmail(this IGuardClause guardClause, string email, string parameterName)
        {
            Guard.Against.NullOrEmpty(email, nameof(email));

            var at = "@";
            if (!email.Contains(at))
            {
                throw new ArgumentException($"Invalid Email - must end in {at}", parameterName);
            }
            

            return email;
        }
    }
}
