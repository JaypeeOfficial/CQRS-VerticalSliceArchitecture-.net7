namespace CQRSample.Features.Users.Exceptions
{
    public class NoUserFound : Exception
    {
        public NoUserFound() : base("No user(s) found") { }

    }
}
