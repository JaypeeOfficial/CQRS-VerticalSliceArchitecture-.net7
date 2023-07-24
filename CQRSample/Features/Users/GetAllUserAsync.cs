using CQRSample.Common.Pagination;
using CQRSample.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSample.Features.Users
{
    public class GetAllUserAsync
    {

        public class GetUserQuery: UserParams, IRequest<PagedList<GetUserResult>>
        {
            public string Search { get; set; }
        }
        public class GetUserResult
        {
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }


        public class Handler : IRequestHandler<GetUserQuery, PagedList<GetUserResult>>
        {
            private readonly StoreContext _context;

            public Handler(StoreContext context)
            {
                _context = context;
            }

            public async Task<PagedList<GetUserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {

                var user = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(request.Search))
                {
                    user = user.Where(x => x.FullName.Contains(request.Search));
                }

                var result = user.Select(x => new GetUserResult
                {
                    FullName = x.FullName,
                    UserName = x.UserName,
                    Password = x.Password,

                });

                return await PagedList<GetUserResult>.CreateAsync(result, request.PageNumber, request.PageSize);

            }

        }


    }
}
