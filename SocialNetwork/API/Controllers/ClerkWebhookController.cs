using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Svix;

namespace API.Controllers;

[ApiController]
[Route("api/webhooks/clerk")]
public class ClerkWebhookController(
    IConfiguration configuration,
    ILogger<ClerkWebhookController> logger
) : RestController
{
    private readonly string _clerkWebhookSecret = configuration["Clerk:Webhooks:Secret"]!;

    [HttpPost]
    public IActionResult Webhooks([FromBody] string payload, [FromHeader] IHeaderDictionary headersDictionary)
    {
        try
        {
            var webhook = new Webhook(_clerkWebhookSecret);
            var headers = new WebHeaderCollection();
            foreach (var header in headersDictionary)
            {
                headers.Add(header.Key, header.Value);
            }

            webhook.Verify(payload, headers);

            var json = JObject.Parse(payload);
            var eventType = json["type"]?.ToString();

            switch (eventType)
            {
                case ClerkEvents.ClerkUserCreated:
                    // var userCreatedPayload = json["data"]!.ToObject<UserCreatedPayload>();
                    break;

                case ClerkEvents.ClerkUserUpdated:
                    // var userUpdatedPayload = json["data"]!.ToObject<UserUpdatedPayload>();
                    break;

                case ClerkEvents.ClerkUserDeleted:
                    // var userDeletedPayload = json["data"]!.ToObject<UserDeletedPayload>();
                    break;
            }

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error processing webhook");
            return BadRequest();
        }
    }
}

public static class ClerkEvents
{
    public const string ClerkUserCreated = "clerk_user_created";
    public const string ClerkUserUpdated = "clerk_user_updated";
    public const string ClerkUserDeleted = "clerk_user_deleted";
}