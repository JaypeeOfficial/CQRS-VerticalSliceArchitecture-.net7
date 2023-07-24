using CQRSample.Data;
using CQRSample.Domain;
using CQRSample.Features.UserRole.Exceptions;
using CQRSample.Features.Users.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSample.Features.Users
{
    public class AddNewUser
    {
        public class AddNewUserCommand : IRequest<Unit>
        {

            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int UserRoleId { get; set; }

        }


        public class Handler : IRequestHandler<AddNewUserCommand, Unit>
        {
            private readonly StoreContext _context;


            public Handler(StoreContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle (AddNewUserCommand command, CancellationToken cancellationToken)
            {
                var userExist = await _context.Users.FirstOrDefaultAsync(x => x.UserName == command.UserName,
                                                                         cancellationToken: cancellationToken);


                var roleExist = await _context.UserRoles.FirstOrDefaultAsync(x => x.Id == command.UserRoleId);

                if(roleExist == null)
                {
                    throw new NoRoleFound();
                }
                       


                if (userExist != null)
                {
                    throw new UserAlreadyExist(command.UserName);
                }

                var users = new User
                {
                    FullName = command.FullName,
                    UserName = command.UserName,
                    Password = command.Password,
                    UserRoleId = command.UserRoleId
                };

                await _context.Users.AddAsync(users, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }

    }
}
