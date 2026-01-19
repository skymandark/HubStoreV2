using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.UserViewModels;

namespace Core.Services.UserServices
{
    public interface IUserService
    {
        Task<UserViewModel> CreateUserAsync(CreateUserViewModel userDto);
        Task<UserViewModel> UpdateUserAsync(string userId, UpdateUserViewModel userDto);
        Task<UserViewModel> GetUserAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, int roleId);
        Task<ValidationResult> ValidatePermissionsAsync(string userId, string permission);
    }
}
