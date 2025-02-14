using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Models;

namespace WebApp.Infrastructure.Authentication
{
    public interface IJWTService
    {
        JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> roles, IList<Claim> userClaims);
    }
}