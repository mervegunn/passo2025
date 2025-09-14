using SportsEcommerceAPI.DTOs;

namespace SportsEcommerceAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<bool> ValidateTokenAsync(string token);
        int GetUserIdFromToken(string token);
    }
}
