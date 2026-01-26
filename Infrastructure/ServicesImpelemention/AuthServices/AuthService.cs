using Core.Domin;
using Core.Dto.Auth;
using Core.Services.AuthServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.ServicesImpelemention.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new AuthResult
                    {
                        Succeeded = false,
                        Message = "User with this email already exists."
                    };
                }

                // Create new user
                var user = new AppUser
                {
                    UserName = request.UserName ?? request.Email,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    EmployeeId = request.EmployeeId,
                    PinCode = request.PinCode,
                    IsAdmin = request.IsAdmin,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return new AuthResult
                    {
                        Succeeded = false,
                        Message = "User registration failed.",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Add user role if admin
                if (request.IsAdmin)
                {
                    await EnsureRoleExists("Admin");
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await EnsureRoleExists("User");
                    await _userManager.AddToRoleAsync(user, "User");
                }

                _logger.LogInformation($"User {user.Email} registered successfully.");

                var userRoles = await _userManager.GetRolesAsync(user);
                var userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    EmployeeId = user.EmployeeId,
                    IsAdmin = user.IsAdmin,
                    Roles = userRoles.ToList()
                };

                return new AuthResult
                {
                    Succeeded = true,
                    Message = "User registered successfully.",
                    User = userInfo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return new AuthResult
                {
                    Succeeded = false,
                    Message = "An error occurred during registration."
                };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return new AuthResult
                    {
                        Succeeded = false,
                        Message = "Invalid email or password."
                    };
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName ?? request.Email,
                    request.Password,
                    request.RememberMe,
                    lockoutOnFailure: true);

                if (!result.Succeeded)
                {
                    var message = result.IsLockedOut ? "Account is locked out." :
                                 result.IsNotAllowed ? "Account is not allowed to sign in." :
                                 result.RequiresTwoFactor ? "Two-factor authentication required." :
                                 "Invalid email or password.";

                    return new AuthResult
                    {
                        Succeeded = false,
                        Message = message
                    };
                }

                _logger.LogInformation($"User {user.Email} logged in successfully.");

                var userRoles = await _userManager.GetRolesAsync(user);
                var userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    EmployeeId = user.EmployeeId,
                    IsAdmin = user.IsAdmin,
                    Roles = userRoles.ToList()
                };

                return new AuthResult
                {
                    Succeeded = true,
                    Message = "Login successful.",
                    User = userInfo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return new AuthResult
                {
                    Succeeded = false,
                    Message = "An error occurred during login."
                };
            }
        }

        public async Task<AuthResult> LoginWithPinCodeAsync(string pinCode)
        {
            try
            {
                if (string.IsNullOrEmpty(pinCode))
                {
                    return new AuthResult
                    {
                        Succeeded = false,
                        Message = "Pin code is required."
                    };
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PinCode == pinCode);
                if (user == null)
                {
                    return new AuthResult
                    {
                        Succeeded = false,
                        Message = "Invalid pin code."
                    };
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                _logger.LogInformation($"User {user.Email} logged in with pin code successfully.");

                var userRoles = await _userManager.GetRolesAsync(user);
                var userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    EmployeeId = user.EmployeeId,
                    IsAdmin = user.IsAdmin,
                    Roles = userRoles.ToList()
                };

                return new AuthResult
                {
                    Succeeded = true,
                    Message = "Login with pin code successful.",
                    User = userInfo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during pin code login");
                return new AuthResult
                {
                    Succeeded = false,
                    Message = "An error occurred during pin code login."
                };
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                throw;
            }
        }

        public async Task<UserInfoDto?> GetCurrentUserAsync()
        {
            try
            {
                // This would typically be injected with IHttpContextAccessor
                // For now, returning null as this needs HttpContext
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                return null;
            }
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                return await _userManager.IsInRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking user role");
                return false;
            }
        }

        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
