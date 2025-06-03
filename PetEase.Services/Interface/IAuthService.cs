using PetEase.Core.DTOs;

namespace PetEase.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthDTOs> RegisterAsync(RegisterDTOs registerDT);
        Task<AuthDTOs> LoginAsync(LoginDTOs loginDTOs);
        Task<AuthDTOs> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
