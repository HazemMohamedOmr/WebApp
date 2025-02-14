using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using WebApp.Core.DTOs;
using WebApp.Core.Models;
using WebApp.Core.Services;
using WebApp.Infrastructure.Authentication;

namespace WebApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJWTService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        public Task<AuthDTO> LoginAsync(LoginRequestDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<AuthDTO> RegisterAsync(RegisterRequestDTO model)
        {
            throw new NotImplementedException();
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return _jwtService.GenerateToken(user, roles, userClaims);
        }
    }
}