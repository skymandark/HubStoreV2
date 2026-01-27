using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();

            var list = new List<AdminUserListVm>();
            foreach (var u in users)
            {
                list.Add(new AdminUserListVm
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsAdmin = u.IsAdmin,
                    LockoutEnabled = u.LockoutEnabled,
                    LockoutEnd = u.LockoutEnd,
                    Roles = (await _userManager.GetRolesAsync(u)).ToList()
                });
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var vm = new AdminUserEditVm
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsAdmin = user.IsAdmin,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminUserEditVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null) return NotFound();

            user.UserName = vm.UserName;
            user.Email = vm.Email;
            user.PhoneNumber = vm.PhoneNumber;
            user.IsAdmin = vm.IsAdmin;
            user.LockoutEnabled = vm.LockoutEnabled;
            user.LockoutEnd = vm.LockoutEnd;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View(vm);
            }

            if (vm.IsAdmin)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                    await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Roles(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.OrderBy(r => r.Name).Select(r => r.Name).ToListAsync();

            var vm = new AdminUserRolesVm
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(r => new AdminRoleItemVm
                {
                    Name = r,
                    IsAssigned = userRoles.Contains(r)
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roles(AdminUserRolesVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var desiredRoles = vm.Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToList();

            var toAdd = desiredRoles.Except(currentRoles).ToList();
            var toRemove = currentRoles.Except(desiredRoles).ToList();

            if (toRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, toRemove);
                if (!removeResult.Succeeded)
                {
                    foreach (var e in removeResult.Errors) ModelState.AddModelError(string.Empty, e.Description);
                    return View(vm);
                }
            }

            if (toAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, toAdd);
                if (!addResult.Succeeded)
                {
                    foreach (var e in addResult.Errors) ModelState.AddModelError(string.Empty, e.Description);
                    return View(vm);
                }
            }

            user.IsAdmin = desiredRoles.Contains("Admin") || user.IsAdmin;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lock(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = null;
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
