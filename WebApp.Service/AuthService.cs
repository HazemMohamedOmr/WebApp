using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;
using WebApp.Core.Models;
using WebApp.Core.Repositories;
using WebApp.Core.Services;
using WebApp.Infrastructure.Authentication;
using WebApp.Infrastructure.Exceptions;

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

    public async Task<IServiceResponse<AuthDTO>> LoginAsync(LoginRequestDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return ServiceResponse<AuthDTO>.Fail("Email or Password is incorrect!", StatusCodes.Status400BadRequest);

        var jwtSecurityToken = await CreateJwtToken(user);
        var rolesList = await _userManager.GetRolesAsync(user);

        var authDto = new AuthDTO
        {
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ExpiresOn = jwtSecurityToken.ValidTo,
            Roles = [.. rolesList]
        };

        return ServiceResponse<AuthDTO>.Success(authDto);
    }

    public async Task<IServiceResponse<AuthDTO>> RegisterAsync(RegisterRequestDTO model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return ServiceResponse<AuthDTO>.Fail("Email is already registered!", StatusCodes.Status400BadRequest);

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
                return ServiceResponse<AuthDTO>.Fail("Failed to register!", StatusCodes.Status400BadRequest, result.Errors);

            await _userManager.AddToRoleAsync(user, model.Role);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return ServiceResponse<AuthDTO>.Fail(ex.Message);
        }

        var jwtSecurityToken = await CreateJwtToken(user);

        var authDto = new AuthDTO
        {
            Email = user.Email,
            ExpiresOn = jwtSecurityToken.ValidTo,
            IsAuthenticated = true,
            Roles = new List<string> { model.Role }, // Supposing user can only have 1 role, Get all roles if needed
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return ServiceResponse<AuthDTO>.Success(authDto);
    }

    //public async Task<string> AddRoleAsync(AddRoleModel model)
    //{
    //    var user = await _userManager.FindByIdAsync(model.UserId);

    //    if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
    //        return "Invalid user ID or Role";

    //    if (await _userManager.IsInRoleAsync(user, model.Role))
    //        return "User already assigned to this role";

    //    var result = await _userManager.AddToRoleAsync(user, model.Role);

    //    return result.Succeeded ? string.Empty : "Something went wrong";
    //}

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        return _jwtService.GenerateToken(user, roles, userClaims);
    }
}