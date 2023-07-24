namespace CQRSample.Features.UserRole.Exceptions
{
    public class UserRoleAlreadyExist : Exception
    {

        public UserRoleAlreadyExist(string role) : base($"Username: {role} already exist") { }

    }
}
