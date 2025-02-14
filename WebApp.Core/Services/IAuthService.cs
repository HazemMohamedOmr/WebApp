using WebApp.Core.DTOs;

namespace WebApp.Core.Services;

public interface IAuthService
{
    Task<AuthDTO> RegisterAsync(RegisterRequestDTO model);
    Task<AuthDTO> LoginAsync(LoginRequestDTO model);
}