using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRSample.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {

    }
}
