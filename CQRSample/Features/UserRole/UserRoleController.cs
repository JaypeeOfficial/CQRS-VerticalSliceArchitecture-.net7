using CQRSample.Common;
using CQRSample.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CQRSample.Features.UserRole.AddNewUserRole;

namespace CQRSample.Features.UserRole
{

    public class UserRoleController : BaseApiController
    {
        private readonly IMediator _mediator;
        public UserRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddNewUserRole")]
        public async Task<IActionResult> AddNewUserRole(AddNewUserRoleCommand command)
        {
            var response = new QueryOrCommandResult<object>();
            try
            {
                await _mediator.Send(command);
                response.Success = true;
                return Ok("Successfully added new user role!");

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Messages.Add(e.Message);
                return Conflict(response);
            }
        }



    }
}
