using Microsoft.AspNetCore.Identity;

namespace MauiApp6.Services.AuthenticationServices.JWTService
{
    public interface IJWTService
    {
        string GenerateJWTToken(IdentityUser user);
    }

}
