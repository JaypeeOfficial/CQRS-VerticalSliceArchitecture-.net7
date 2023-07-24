using CQRSample.Data;
using CQRSample.Features.UserRole.Exceptions;
using CQRSample.Features.Users.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSample.Features.UserRole
{
    public class AddNewUserRole
    {

        public class AddNewUserRoleCommand : IRequest<Unit>
        {
            public string RoleCode { get; set; }
            public string RoleName { get; set; }
        }


        public class Handler : IRequestHandler<AddNewUserRoleCommand, Unit>
        {
            private readonly StoreContext _context;
            public Handler(StoreContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(AddNewUserRoleCommand command, CancellationToken cancellationToken)
            {
                var useRoleExist = await _context.UserRoles.FirstOrDefaultAsync(x => x.RoleName == command.RoleName,
                                                                         cancellationToken: cancellationToken);

                if (useRoleExist != null)
                {
                    throw new UserRoleAlreadyExist(command.RoleName);
                }

                var roles = new Domain.UserRole
                {
                    RoleCode = command.RoleCode,
                    RoleName = command.RoleName
                };

                await _context.UserRoles.AddAsync(roles, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }

    }

}
