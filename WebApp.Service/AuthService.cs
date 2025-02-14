using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.DTOs;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Authentication;

namespace WebApp.Service;

public class AuthService : IAuthService
{
    private readonly IJWTService _jwtService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IJWTService jwtService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthDTO> LoginAsync(LoginRequestDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return new AuthDTO { Message = "Email or Password is incorrect!" };

        var jwtSecurityToken = await CreateJwtToken(user);
        var rolesList = await _userManager.GetRolesAsync(user);

        return new AuthDTO
        {
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ExpiresOn = jwtSecurityToken.ValidTo,
            Roles = [.. rolesList]
        };
    }

    public async Task<AuthDTO> RegisterAsync(RegisterRequestDTO model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthDTO { Message = "Email is already registered!" };

        var user = new ApplicationUser
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.FirstName + model.LastName
        };

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new AuthDTO
                {
                    Errors = result.Errors,
                    Message = "Failed to register!"
                };

            await _userManager.AddToRoleAsync(user, model.Role);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new AuthDTO
            {
                Message = ex.Message
            };
        }

        var jwtSecurityToken = await CreateJwtToken(user);

        return new AuthDTO
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Roles = new List<string> { model.Role }, // Supposing user can only have 1 role, Get all roles if needed
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    //public async Task<string> AddRoleAsync(AddRoleModel model)
    //{
    //    var user = await _userManager.FindByIdAsync(model.UserId);

    //    if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
    //        return "Invalid user ID or Role";

    //    if (await _userManager.IsInRoleAsync(user, model.Role))
    //        return "User already assigned to this role";

    //    var result = await _userManager.AddToRoleAsync(user, model.Role);

    //    return result.Succeeded ? string.Empty : "Sonething went wrong";
    //}

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return _jwtService.GenerateToken(user, roles, userClaims);
    }
}