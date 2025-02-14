using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.Core.Models;

namespace WebApp.Infrastructure.Authentication;

public interface IJWTService
{
    JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> roles, IList<Claim> userClaims);
}