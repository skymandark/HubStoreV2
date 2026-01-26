using System.Collections.Generic;

namespace Core.Dto.Auth
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public UserInfoDto? User { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? EmployeeId { get; set; }
        public string? PinCode { get; set; }
        public bool IsAdmin { get; set; } = false;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }

    public class PinLoginRequest
    {
        public string PinCode { get; set; } = string.Empty;
    }

    public class UserInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? EmployeeId { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
