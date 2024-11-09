using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.SeedWorks;
using SharedKernel.Utils;

namespace API.Controllers;

[ApiController]
[Route("api/whoami")]
public class WhoamiController : RestController
{
    [Authorize]
    [HttpGet("private")]
    public ActionResult<Response> GetPrivate()
    {
        return ResponseFactory.Ok(UserId);
    }
}