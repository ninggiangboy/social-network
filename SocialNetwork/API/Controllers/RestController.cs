using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SharedKernel.SeedWorks;

namespace API.Controllers;

public class RestController : ControllerBase
{
    protected string UserId
    {
        get
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            return userId;
        }
    }
}