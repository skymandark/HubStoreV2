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
    public class AdminRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminRolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();

            var list = await Task.WhenAll(roles.Select(async r => new AdminRoleListVm
            {
                Id = r.Id,
                Name = r.Name,
                UsersCount = (await _userManager.GetUsersInRoleAsync(r.Name)).Count
            }));

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AdminRoleCreateVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminRoleCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _roleManager.RoleExistsAsync(vm.Name))
            {
                ModelState.AddModelError(string.Empty, "Role already exists");
                return View(vm);
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(vm.Name));
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }
    }
}
