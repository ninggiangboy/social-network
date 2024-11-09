using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Clerk.Net.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Authentication;

public static class ClerkExtensions
{
    public static void AddClerkAuthentication(
        this IServiceCollection services, IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        // services.AddClerkApiClient(config => { config.SecretKey = configuration["Clerk:SecretKey"]!; });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Assume publicKey is your Clerk instance's public RSA key in PEM format
                var pemFormat = configuration["ClerkPublicKeyPemFormat"];

                if (pemFormat == null)
                {
                    throw new Exception(nameof(pemFormat));
                }

                RSA rsa;

                try
                {
                    rsa = RSA.Create();
                    rsa.ImportFromPem(pemFormat.ToCharArray());
                }
                catch (ArgumentException)
                {
                    throw new Exception("Invalid PEM format");
                }
                catch (CryptographicException)
                {
                    throw new Exception("Invalid PEM format");
                }

                options.Authority = configuration["ClerkAuthority"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var azp = context.Principal?.FindFirstValue("azp");
                        if (string.IsNullOrEmpty(azp) || !azp.Equals(configuration["ClerkAuthorizedParty"]))
                        {
                            context.Fail("AZP Claim is invalid or missing");
                            return;
                        }

                        await SetClaims(context, configuration, httpClientFactory).ConfigureAwait(false);
                    },
                };
            });
    }

    // Claims can be saved in JWT via Clerk.com
    private static async Task SetClaims(TokenValidatedContext context, IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        try
        {
            // Assuming 'configuration' and 'context' are available in the scope
            var clerkApiKey = configuration["ClerkSecretKey"];
            if (string.IsNullOrEmpty(clerkApiKey))
            {
                throw new InvalidOperationException("Clerk API key is not configured properly.");
            }

            var userIdClaim =
                context.Principal.Claims.FirstOrDefault(c =>
                    c.Type == "user_id" || c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new InvalidOperationException("User ID claim is not available.");
            }

            string userId = userIdClaim.Value;
            string clerkApiUrl = $"https://api.clerk.com/v1/users/{userId}";

            using var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clerkApiKey);

            var response = await client.GetAsync(clerkApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                // handle error as needed
                Console.WriteLine("Failed to retrieve user details from Clerk.");
                return;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            var user = jsonDoc.RootElement;

            var id = user.GetProperty("id").GetString();
            var emailAddress = user.GetProperty("email_addresses")[0].GetProperty("email_address").GetString();
            var userName = user.GetProperty("username").GetString();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, emailAddress),
            };

            if (context.Principal.Identity is ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.AddClaims(claims);
            }
        }
        catch (Exception ex)
        {
            // handle error as needed
            Console.WriteLine($"Error in VerifyTokenWithClerkAsync: {ex.Message}");
        }
    }
}