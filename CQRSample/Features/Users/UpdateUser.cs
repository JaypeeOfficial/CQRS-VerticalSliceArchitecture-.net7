using CQRSample.Data;
using CQRSample.Features.Users.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSample.Features.Users
{
    public class UpdateUser
    {
        public class UpdateUserCommand : IRequest<Unit>
        {

            public int Id { get; set; }
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }


            public class Handler : IRequestHandler<UpdateUserCommand, Unit>
            {
                private readonly StoreContext _context;

                public Handler(StoreContext context)
                {
                    _context = context;
                }

                public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellation)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id, cancellation);

                    if (user == null)
                        throw new NoUserFound();


                    user.FullName = command.FullName;   
                    user.UserName = command.UserName;
                    user.Password = command.Password;

                    await _context.SaveChangesAsync(cancellation);

                    return Unit.Value;

                }

            }

        }

    }
}
