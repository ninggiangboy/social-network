using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Securities;

public static class IdentityExtensions
{
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddAuthentication().AddCookie();
        services.AddAuthorization();
        services.AddIdentityCore<Profile>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.AllowedForNewUsers = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<SocialNetworkDbContext>()
            .AddApiEndpoints();
    }

    public static void MapIdentityApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("identity").MapIdentityApi<Profile>();
    }
}