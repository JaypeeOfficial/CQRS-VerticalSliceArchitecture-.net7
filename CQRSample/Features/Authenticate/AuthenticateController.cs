using CQRSample.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CQRSample.Features.Authenticate.AuthenticateUser;

namespace CQRSample.Features.Authenticate
{

    public class AuthenticateController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator meadiator)
        {          
            _mediator = meadiator;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]

        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            var response = new QueryOrCommandResult<AuthenticateResponse>();
            try
            {
                var result = await _mediator.Send(request);
                response.Success = true;
                response.Data = result;
                return Ok(response);
            }
            catch (Exception e)
            {
                return Conflict(new
                {
                    e.Message
                });
            }
        }

        }
}
