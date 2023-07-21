namespace CQRSample.Features.Users.Exceptions
{
    public class UserAlreadyExist : Exception
    {

        public UserAlreadyExist(string username) : base($"Username: {username} already exist") { }

    }
}
