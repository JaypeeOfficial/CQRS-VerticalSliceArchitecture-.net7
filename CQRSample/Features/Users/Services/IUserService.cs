using CQRSample.Domain;

namespace CQRSample.Features.Users.Services
{
    public interface IUserService
    {
        void AddNewUser(User users);
        Task SaveAsync();
    }
}
