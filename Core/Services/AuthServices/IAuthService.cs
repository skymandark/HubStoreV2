using Core.Domin;
using Core.Dto.Auth;
using System.Threading.Tasks;

namespace Core.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<AuthResult> LoginWithPinCodeAsync(string pinCode);
        Task LogoutAsync();
        Task<UserInfoDto?> GetCurrentUserAsync();
        Task<bool> IsUserInRoleAsync(string userId, string role);
    }
}
