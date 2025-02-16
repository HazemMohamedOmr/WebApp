using WebApp.Core.DTOs;
using WebApp.Core.Interfaces;

namespace WebApp.Core.Services;

public interface IAuthService
{
    Task<IServiceResponse<AuthDTO>> RegisterAsync(RegisterRequestDTO model);
    Task<IServiceResponse<AuthDTO>> LoginAsync(LoginRequestDTO model);
}