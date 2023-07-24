namespace CQRSample.Features.UserRole.Exceptions
{
    public class NoRoleFound : Exception
    {
        public NoRoleFound() : base("No user role(s) found") { }

    }
}
