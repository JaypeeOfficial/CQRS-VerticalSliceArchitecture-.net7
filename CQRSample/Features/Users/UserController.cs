using CQRSample.Common;
using CQRSample.Common.Extension;
using CQRSample.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CQRSample.Features.Users.GetAllUserAsync;

namespace CQRSample.Features.Users
{
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("AddNewUser")]
        public async Task<IActionResult> AddNewUser(AddNewUser.AddNewUserCommand command)
        {
            var response = new QueryOrCommandResult<object>();
            try
            {
                await _mediator.Send(command);
                response.Success = true;
                return Ok("Successfully added new user!");

            }
            catch (System.Exception e)
            {
                response.Success = false;
                response.Messages.Add(e.Message);
                return Conflict(response);
            }
        }


        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserParams userParams)
        {
            try
            {
                var query = new GetAllUserAsync.GetUserQuery
                {
                    PageNumber = userParams.PageNumber,
                    PageSize = userParams.PageSize
                };

                var users = await _mediator.Send(query);

                Response.AddPaginationHeader(users.CurrentPage, users.TotalCount, users.TotalPages, users.PageSize, users.HasNextPage, users.HasPreviousPage);

                var result = new
                {
                    users,
                    users.PageSize,
                    users.CurrentPage,
                    users.TotalCount,
                    users.TotalPages,
                    users.HasPreviousPage,
                    users.HasNextPage
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                return Conflict(new { message = e.Message });
            }
        }



    }
}
