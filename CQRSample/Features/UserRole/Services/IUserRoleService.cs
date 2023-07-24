using CQRSample.Domain;

namespace CQRSample.Features.UserRole.Services
{
    public interface IUserRoleService
    {
        void AddNewUserRole(Domain.UserRole roles);
        Task SaveAsync();

    }
}
