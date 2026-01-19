//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Core.Domin;
//using Core.Domin.Approval;
//using Core.Domin.IdentityData;
//using Core.Dto.UserDetalis;
//using Core.Dtos.UserDetail;
//using Core.ViewModel;
//using Core.ViewModelUser;
//using DevExtreme.AspNet.Data;
//using HubStoreV2.Models;
//using Infrastructure.Data;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using Repository;


//namespace HubStore.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly ApplicationIdentityDbContext _identityContext;
//        private readonly ApplicationDbContext _context;
//        public AdminController(ApplicationIdentityDbContext identityContext, ApplicationDbContext context)
//        {
//            _identityContext = identityContext;
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet]
//        public IActionResult Users()
//        {
//            return View();
//        }
//        public IActionResult ReadUsers(DataSourceLoadOptions loadOptions)
//        {
//            var users = _identityContext.Users.AsQueryable().Select(s => new UserVm()
//            {
//                UserId = s.Id,
//                UserSerial = s.UserSerial ?? 0,
//                EmployeeId = s.EmployeeId,
//                UserName = s.UserName,
//                Email = s.Email,
//                PhoneNumber = s.PhoneNumber,
//                Active = s.LockoutEnd.GetValueOrDefault() < DateTimeOffset.Now
//            }).ToList();
//            var data = DataSourceLoader.Load(users, loadOptions);
//            return Json(data);
//        }

//        [HttpGet]
//        public IActionResult Roles()
//        {
//            return View();
//        }
//        public IActionResult ReadRoles(DataSourceLoadOptions loadOptions)
//        {
//            var roles = _identityContext.Roles.AsQueryable().Select(s => new RoleVm()
//            {
//                Id = s.Id,
//                Name = s.Name,
//                ApplicationName = s.Applications.AppName,
//                RoleType = s.RoleType,
//                RoleValue = s.RoleValue
//            }).ToList();
//            var data = DataSourceLoader.Load(roles, loadOptions);
//            return Json(data);
//        }

//        [HttpGet]
//        [Route("[controller]/[action]")]
//        [Route("[controller]/[action]/{id}")]
//        public async Task<IActionResult> Role(string id)
//        {
//            if (string.IsNullOrWhiteSpace(id))
//            {
//                var newRole = new RoleDto
//                {
//                    RoleType = "",
//                    RoleValue = "",
//                    Applications = _identityContext.Applications.Select(x => new ApplicationDto()
//                    {
//                        Id = x.Id,
//                        AppName = x.AppName
//                    }).ToList(),
//                    ApplicationId = 1
//                };
//                return View(newRole);
//            }
//            var role = await _identityContext.Roles.FindAsync(id.ToString());
//            var roleDto = new RoleDto
//            {
//                Id = role.Id,
//                ApplicationId = role.ApplicationId,
//                Name = role.Name,
//                RoleType = role.RoleType,
//                RoleValue = role.RoleValue,
//                Applications = _identityContext.Applications.Select(x => new ApplicationDto
//                {
//                    Id = x.Id,
//                    AppName = x.AppName,
//                }).ToList(),
//            };
//            return View(roleDto);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Role(RoleDto role)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(role);
//            }
//            if (string.IsNullOrWhiteSpace(role.Id))
//            {
//                var menuIds = new List<int>();
//                if (!string.IsNullOrWhiteSpace(role.RoleValue) && role.RoleValue.StartsWith("menus"))
//                {
//                    var menus = role.RoleValue.Split("#")[1].Split(",").ToList();
//                    foreach (var item in menus)
//                    {
//                        menuIds.Add(Convert.ToInt32(item));
//                    }
//                    role.RoleValue = JsonConvert.SerializeObject(menuIds);
//                }
//                var entityRole = new UserIdentityRole
//                {
//                    Id = Guid.NewGuid().ToString(),
//                    Name = role.Name,
//                    NormalizedName = role.Name.ToUpper(),
//                    ApplicationId = role.ApplicationId,
//                    RoleType = role.RoleType,
//                    RoleValue = role.RoleValue
//                };
//                await _identityContext.Roles.AddAsync(entityRole);
//                await _identityContext.SaveChangesAsync();
//                SuccessNotification($"تم إضافة الصلاحية {role.Name} بنجاح", "تهانينا");
//            }
//            else
//            {
//                var menuIds = new List<int>();
//                if (!string.IsNullOrWhiteSpace(role.RoleValue) && role.RoleValue.StartsWith("menus"))
//                {
//                    var menus = role.RoleValue.Split("#")[1].Split(",").ToList();
//                    foreach (var item in menus)
//                    {
//                        menuIds.Add(Convert.ToInt32(item));
//                    }
//                    role.RoleValue = JsonConvert.SerializeObject(menuIds);
//                }
//                var entityRole = await _identityContext.Roles.FindAsync(role.Id);
//                if (entityRole != null)
//                {
//                    entityRole.NormalizedName = role.Name.ToUpper();
//                    entityRole.Name = role.Name;
//                    entityRole.RoleValue = role.RoleValue;
//                    entityRole.RoleType = role.RoleType;
//                    entityRole.ApplicationId = role.ApplicationId;
//                }
//                await _identityContext.SaveChangesAsync();
//                SuccessNotification($"تم تعديل الصلاحية {role.Name} بنجاح", "تهانينا");
//            }
//            return RedirectToAction(nameof(Roles));
//        }

//        [HttpGet]
//        public async Task<IActionResult> RoleDelete(string id)
//        {
//            if (string.IsNullOrWhiteSpace(id)) return NotFound();

//            var roleDto = await _identityContext.Roles.FindAsync(id.ToString());
//            if (roleDto == null) return NotFound();

//            return View(roleDto);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> RoleDelete(RoleDto role)
//        {
//            var entityRole = await _identityContext.Roles.FindAsync(role.Id);

//            if (entityRole == null) return RedirectToAction(nameof(Roles));

//            _identityContext.Roles.Remove(entityRole);
//            await _identityContext.SaveChangesAsync();

//            return RedirectToAction(nameof(Roles));
//        }

//        public async Task<IActionResult> GetMenus(int applicationId, string userId)
//        {
//            var mainMenus = _identityContext.Menus.Where(x => x.ApplicationId == applicationId).Select(x => x.MainMenu).Distinct();

//            var menus = await _identityContext.Menus.Where(x => x.ApplicationId == applicationId).Select(x => new MenuVm
//            {
//                text = x.MenuName,
//                id = x.MenuId,
//                MenuUrl = x.MenuUrl,
//                MainMenu = x.MainMenu,
//                IsGroup = mainMenus.Contains(x.MenuId)
//            }).ToListAsync();
//            return Json(menus);
//        }

//        public IActionResult GetMenusSelected(string roleId, int applicationId)
//        {
//            var mainMenus = _identityContext.Menus.Select(x => x.MainMenu).Distinct();
//            var roleClaimValue = _identityContext.Roles.Where(x =>
//                x.Id == roleId).Select(x => x.RoleValue).FirstOrDefault();

//            var menusFromDb = JsonConvert.DeserializeObject<List<int>>(roleClaimValue ?? string.Empty);

//            var menuFromDb = _identityContext.Menus.Where(x => menusFromDb.Contains(x.MenuId)).ToList();

//            var menuIdsFromRegistered = menuFromDb.Where(w => w.IsMain == false).Select(x => x.MenuId).ToList();

//            var menus = _identityContext.Menus.Where(x => x.ApplicationId == 1).Select(x => new MenuVm
//            {
//                text = x.MenuName,
//                id = x.MenuId,
//                MenuUrl = x.MenuUrl,
//                MainMenu = x.MainMenu,
//                IsGroup = mainMenus.Contains(x.MenuId),
//                selected = menuIdsFromRegistered.Contains(x.MenuId)
//            }).ToList();

//            return Json(menus);
//        }

//        [HttpGet]
//        public IActionResult RoleUsers(string id)
//        {
//            ViewBag.RoleId = id;
//            return View();
//        }

//        [HttpGet]
//        public IActionResult ReadRoleUsers(string id, DataSourceLoadOptions loadOptions)
//        {
//            var roleUsers =
//                (from userRole in _identityContext.UserRoles.AsQueryable().Where(x => x.RoleId == id)
//                 from role in _identityContext.Roles.AsQueryable().Where(x => x.Id == userRole.RoleId)
//                 from user in _identityContext.Users.AsQueryable().Where(x => x.Id == userRole.UserId)
//                 select new UserRoleVm()
//                 {
//                     RoleName = role.Name,
//                     UserName = user.UserName,
//                     RoleId = userRole.RoleId,
//                     UserId = userRole.UserId
//                 }).ToList();
//            var data = DataSourceLoader.Load(roleUsers, loadOptions);
//            return Json(data);
//        }

//        [HttpPost]
//        public IActionResult DeleteUserRole(string roleId, string userId)
//        {
//            try
//            {
//                var role = _identityContext.UserRoles.AsQueryable()
//                    .FirstOrDefault(x => x.UserId == userId && x.RoleId == roleId);
//                if (role != null)
//                {
//                    _identityContext.UserRoles.Remove(role);
//                    _identityContext.SaveChanges();
//                }
//                return Json(new { success = true, message = "تم حذف صلاحية المستخدم بنجاح" });
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                return Json(new { success = false, message = "عفواً حدث خطأ أثناء الحذف" });
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UserRolesDelete(UserRolesDto role)
//        {
//            var userRole = await _identityContext.UserRoles.Where(x => x.RoleId == role.RoleId && x.UserId == role.UserId).FirstOrDefaultAsync();

//            if (userRole == null) return RedirectToAction(nameof(Roles));

//            _identityContext.UserRoles.Remove(userRole);
//            await _identityContext.SaveChangesAsync();

//            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
//        }

//        public class PagedList<T> where T : class
//        {
//            public PagedList()
//            {
//                Data = new List<T>();
//            }

//            public List<T> Data { get; }

//            public int TotalCount { get; set; }

//            public int PageSize { get; set; }
//        }

//        public virtual async Task<PagedList<RoleDto>> GetUserRolesAsync(string userId, int page = 1, int pageSize = 10)
//        {
//            var pagedList = new PagedList<RoleDto>();
//            var roles = from r in _identityContext.Roles
//                        join ur in _identityContext.UserRoles on r.Id equals ur.RoleId
//                        where ur.UserId.Equals(userId)
//                        select r;

//            var userIdentityRoles = await roles.Take(pageSize).Skip((page - 1) * pageSize).OrderBy(x => x.Id)
//                .Select(x => new RoleDto
//                {
//                    Id = x.Id,
//                    ApplicationId = x.ApplicationId,
//                    Name = x.Name,
//                    RoleType = x.RoleType,
//                    RoleValue = x.RoleValue
//                }).ToListAsync();

//            pagedList.Data.AddRange(userIdentityRoles);
//            pagedList.TotalCount = await roles.CountAsync();
//            pagedList.PageSize = pageSize;

//            return pagedList;
//        }

//        public virtual async Task<UserRolesDto> UserRolesAsync(string userId, int page = 1, int pageSize = 10)
//        {

//            var userIdentityRoles = await GetUserRolesAsync(userId, page, pageSize);

//            var user = await _identityContext.Users.FindAsync(userId);

//            return new UserRolesDto
//            {
//                UserName = user.UserName,
//                Roles = userIdentityRoles.Data
//            };
//        }

//        [HttpGet]
//        public async Task<IActionResult> UserRoles(string id, int? page)
//        {
//            if (string.IsNullOrWhiteSpace(id)) return NotFound();

//            var roles = await _identityContext.Roles.ToListAsync();
//            var roleDtos = roles.Select(x => new RoleDto
//            {
//                Id = x.Id,
//                ApplicationId = x.ApplicationId,
//                Name = x.Name,
//                RoleType = x.RoleType,
//                RoleValue = x.RoleValue
//            });

//            var userRoles = await UserRolesAsync(id.ToString(), page ?? 1);
//            userRoles.UserId = id;
//            userRoles.RolesList = roleDtos.Select(x => new SelectItemDto(x.Id.ToString(), x.Name)).ToList();


//            return View(userRoles);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UserRoles(UserRolesDto role)
//        {
//            var userRole = new UserIdentityUserRole
//            {
//                UserId = role.UserId,
//                RoleId = role.RoleId
//            };

//            await _identityContext.UserRoles.AddAsync(userRole);
//            await _identityContext.SaveChangesAsync();

//            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
//        }
//        [HttpGet]
//        public IActionResult UserProfile()
//        {
//            var newUser = new UserDto();
//            newUser.UserSerial = (_identityContext.Users.OrderByDescending(o => o.UserSerial).Select(s => s.UserSerial).Max() ?? 0) + 1;
//            newUser.Applications = _identityContext.Applications.Select(x => new ApplicationDto()
//            {
//                Id = x.Id,
//                AppName = x.AppName
//            }).ToList();
//            return View("UserProfile", newUser);
//        }

//        [HttpGet]
//        [Route("[controller]/UserProfile/{id}")]
//        public async Task<IActionResult> UserProfile(string id)
//        {
//            var entityUser = await _identityContext.Users.FindAsync(id);
//            if (entityUser == null) return NotFound();
//            var user = new UserDto
//            {
//                Id = entityUser.Id,
//                UserName = entityUser.UserName,
//                Email = entityUser.Email,
//                EmailConfirmed = true,
//                EmployeeId = entityUser.EmployeeId,
//                LockoutEnabled = entityUser.LockoutEnabled,
//                LockoutEnd = entityUser.LockoutEnd,
//                AccessFailedCount = entityUser.AccessFailedCount,
//                IsAdmin = entityUser.IsAdmin,
//                PhoneNumber = entityUser.PhoneNumber,
//                PhoneNumberConfirmed = true,
//                PinCode = entityUser.PinCode,
//                TwoFactorEnabled = entityUser.TwoFactorEnabled,
//                UserSerial = entityUser.UserSerial ?? 0
//            };
//            user.Applications = _identityContext.Applications.Select(x => new ApplicationDto()
//            {
//                Id = x.Id,
//                AppName = x.AppName
//            }).ToList();
//            user.BranchesList = await GetUserBranches(5, id);
//            user.RolesList = GetUserRoles(id.ToString());
//            return View("UserProfile", user);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UserProfile(UserDto user)
//        {
//            if (!ModelState.IsValid)
//            {
//                user.Applications = _identityContext.Applications.Select(x => new ApplicationDto()
//                {
//                    Id = x.Id,
//                    AppName = x.AppName
//                }).ToList();
//                return View(user);
//            }

//            if (string.IsNullOrEmpty(user.Id))
//            {
//                 ============ إنشاء مستخدم جديد ============

//                 تحديد UserSerial
//                user.UserSerial = _identityContext.Users.Any()
//                    ? (_identityContext.Users.Max(x => x.UserSerial) ?? 0) + 1
//                    : 1;

//                إنشاء كيان المستخدم
//               var hasher = new PasswordHasher<AppUser>();
//                var userEntity = new AppUser
//                {
//                    Id = Guid.NewGuid().ToString(),
//                    UserName = user.UserName,
//                    NormalizedUserName = user.UserName.ToUpper(),
//                    Email = user.Email,
//                    NormalizedEmail = user.Email.ToUpper(),
//                    EmailConfirmed = true,
//                    EmployeeId = user.EmployeeId,
//                    LockoutEnabled = user.LockoutEnabled,
//                    LockoutEnd = user.LockoutEnd,
//                    AccessFailedCount = user.AccessFailedCount,
//                    IsAdmin = user.IsAdmin,
//                    SecurityStamp = Guid.NewGuid().ToString(),
//                    ConcurrencyStamp = Guid.NewGuid().ToString(),
//                    PhoneNumber = user.PhoneNumber,
//                    PhoneNumberConfirmed = true,
//                    PinCode = user.PinCode,
//                    TwoFactorEnabled = user.TwoFactorEnabled,
//                    UserSerial = user.UserSerial
//                };

//                تشفير كلمة المرور
//                userEntity.PasswordHash = hasher.HashPassword(userEntity, user.Password);

//                حفظ المستخدم
//                await _identityContext.Users.AddAsync(userEntity);
//                await _identityContext.SaveChangesAsync();

//                 ✅ حفظ الفروع والصلاحيات(هذا كان مفقود!)
//                var branches = JsonConvert.DeserializeObject<List<UserBranchesDto>>(user.Branches ?? "[]");
//                var roles = JsonConvert.DeserializeObject<List<UserRolesDto>>(user.Roles ?? "[]");

//                if (branches != null && branches.Any() || roles != null && roles.Any())
//                {
//                    AddBranchesAndRolesToUser(branches ?? new List<UserBranchesDto>(),
//                                             roles ?? new List<UserRolesDto>(),
//                                             userEntity.Id);
//                }

//                return RedirectToAction(nameof(UserProfile), new { Id = userEntity.Id });
//            }
//            else
//            {
//                 ============ تحديث مستخدم موجود ============

//                var userEntity = await _identityContext.Users.FindAsync(user.Id);

//                if (userEntity == null)
//                    return NotFound();

//                تحديث البيانات الأساسية
//                userEntity.UserName = user.UserName;
//                userEntity.NormalizedUserName = user.UserName.ToUpper();
//                userEntity.Email = user.Email;
//                userEntity.NormalizedEmail = user.Email.ToUpper();
//                userEntity.EmployeeId = user.EmployeeId;
//                userEntity.LockoutEnabled = user.LockoutEnabled;
//                userEntity.LockoutEnd = user.LockoutEnd;
//                userEntity.AccessFailedCount = user.AccessFailedCount;
//                userEntity.IsAdmin = user.IsAdmin;
//                userEntity.SecurityStamp = Guid.NewGuid().ToString();
//                userEntity.ConcurrencyStamp = Guid.NewGuid().ToString();
//                userEntity.PhoneNumber = user.PhoneNumber;
//                userEntity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
//                userEntity.PinCode = user.PinCode;
//                userEntity.TwoFactorEnabled = user.TwoFactorEnabled;
//                userEntity.UserSerial = user.UserSerial;

//                حفظ التحديثات
//                _identityContext.Users.Update(userEntity);
//                await _identityContext.SaveChangesAsync();

//                 ✅ تحديث الفروع والصلاحيات
//                var branches = JsonConvert.DeserializeObject<List<UserBranchesDto>>(user.Branches ?? "[]");
//                var roles = JsonConvert.DeserializeObject<List<UserRolesDto>>(user.Roles ?? "[]");

//                UpdateBranchesAndRolesToUser(branches ?? new List<UserBranchesDto>(),
//                                             roles ?? new List<UserRolesDto>(),
//                                             user.Id);

//                return RedirectToAction(nameof(UserProfile), new { Id = userEntity.Id });
//            }
//        }
//        public void AddBranchesAndRolesToUser(List<UserBranchesDto> branches, List<UserRolesDto> roles, string userId)
//        {
//            AddBranchesToUser(branches, userId);
//            AddRolesToUser(roles, userId);
//        }
//        public void UpdateBranchesAndRolesToUser(List<UserBranchesDto> branches, List<UserRolesDto> roles, string userId)
//        {
//            UpdateBranchesToUser(branches, userId);
//            UpdateRolesToUser(roles, userId);
//        }
//        public void AddBranchesToUser(List<UserBranchesDto> branches, string userId)
//        {
//            foreach (var item in branches)
//            {
//                var userValidation = new UserValidations()
//                {
//                    ApplicationId = item.ApplicationId,
//                    UserId = userId,
//                    ValidValue = item.BranchId,
//                    ValidId = 1
//                };
//                _identityContext.UserValidations.Add(userValidation);
//                _identityContext.SaveChanges();
//            }
//        }
//        public void AddRolesToUser(List<UserRolesDto> roles, string userId)
//        {
//            foreach (var item in roles)
//            {
//                var manageRole = new UserIdentityUserRole()
//                {
//                    UserId = userId,
//                    RoleId = item.RoleId
//                };
//                _identityContext.UserRoles.Add(manageRole);
//                _identityContext.SaveChanges();
//            }
//        }
//        public void UpdateBranchesToUser(List<UserBranchesDto> branches, string userId)
//        {
//            var oldBranches = _identityContext.UserValidations.Where(x => x.UserId == userId).ToList();
//            _identityContext.UserValidations.RemoveRange(oldBranches);
//            _identityContext.SaveChanges();

//            foreach (var item in branches)
//            {
//                var userValidation = new UserValidations()
//                {
//                    ApplicationId = 5,
//                    UserId = userId,
//                    ValidValue = item.BranchId,
//                    ValidId = 1
//                };
//                _identityContext.UserValidations.Add(userValidation);
//                _identityContext.SaveChanges();
//            }
//        }
//        public void UpdateRolesToUser(List<UserRolesDto> roles, string userId)
//        {
//            var oldRoles = _identityContext.UserRoles.Where(x => x.UserId == userId).ToList();
//            _identityContext.UserRoles.RemoveRange(oldRoles);
//            _identityContext.SaveChanges();

//            foreach (var item in roles)
//            {

//                var manageRole = new UserIdentityUserRole()
//                {
//                    UserId = userId,
//                    RoleId = item.RoleId
//                };
//                _identityContext.UserRoles.Add(manageRole);
//                _identityContext.SaveChanges();
//            }
//        }

//        public List<UserRoleDto> GetUserRoles(string userId)
//        {
//            var data =
//                (from roles in _identityContext.Roles.AsQueryable()
//                 from application in _identityContext.Applications.AsQueryable()
//                     .Where(x => x.Id == roles.ApplicationId).DefaultIfEmpty()
//                 from userRoles in _identityContext.UserRoles.AsQueryable()
//                     .Where(x => x.RoleId == roles.Id && x.UserId == userId)
//                 select new UserRoleDto()
//                 {
//                     RoleId = roles.Id,
//                     RoleName = roles.Name,
//                     ApplicationId = application != null ? application.Id : 0,
//                     ApplicationName = application != null ? application.AppName : "عام"
//                 }).ToList();
//            return data;
//        }
//        public async Task<IActionResult> CheckEmpId(int? empId)
//        {
//            var exists = await _identityContext.EmployeeData.Where(x => x.BaseId == empId).AnyAsync();
//            if (exists)
//            {
//                var taken = await _identityContext.Users.AsQueryable().AnyAsync(a => a.EmployeeId == empId);
//                if (!taken)
//                {
//                    return Ok(true);
//                }
//            }
//            return Ok(false);
//        }

//        public IActionResult GetRolesByApplicationId(int applicationId, string userId)
//        {
//            var roles = (from role in _identityContext.Roles.AsQueryable()
//                    .Where(x => x.ApplicationId == applicationId)
//                         from userRoles in _identityContext.UserRoles.AsQueryable().Where(x => x.UserId == userId && x.RoleId == role.Id)
//                             .DefaultIfEmpty()
//                         select new RoleVm()
//                         {
//                             Id = role.Id,
//                             Name = role.Name,
//                             IsFound = true
//                         }).ToList();
//            return Json(roles);
//        }






//        new Data

//        public IActionResult UserApprove(string userId, string userName)
//        {
//            var currentApprove = _identityContext.ApprovesUsers.AsQueryable().Where(x => x.UserId == userId)
//                .Select(s => s.ApproveStepId).ToList();
//            var model = new UserApproveDto()
//            {
//                UserId = userId,
//                UserName = userName,
//                Approves =
//                    (from def in _identityContext.ApproveDefnetions.AsQueryable()
//                     from step in _identityContext.ApproveSteps.AsQueryable().Where(x => x.LastProgramaticStep == false && x.Deleted == false && x.ApproveOrder == 1 && x.ApproveDefId == def.Id && currentApprove.All(w => w != x.Id))
//                     select new ApproveDefinitionDto()
//                     {
//                         Id = step.Id,
//                         ApproveName = def.Id + "-" + def.ApprovName + " / " + def.SystemName
//                     }).ToList()
//            };
//            return View(model);
//        }

//        public IActionResult ReadUserApproves(DataSourceLoadOptions loadOptions, string userId)
//        {
//            var userApproves =
//                (from def in _identityContext.ApproveDefnetions.AsQueryable()
//                 from step in _identityContext.ApproveSteps.AsQueryable().Where(x => x.ApproveDefId == def.Id)
//                 from userApprove in _identityContext.ApprovesUsers.AsQueryable()
//                     .Where(x => x.UserId == userId && x.ApproveStepId == step.Id)
//                 select new ApproveDefinitionDto()
//                 {
//                     Id = def.Id,
//                     StepId = userApprove.ApproveStepId,
//                     SystemName = def.SystemName,
//                     ApproveName = def.ApprovName,
//                     TableName = def.TableName,
//                     UserId = userApprove.UserId
//                 }).ToList();
//            var data = DataSourceLoader.Load(userApproves, loadOptions);
//            return Json(data);
//        }

//        public IActionResult ReadApproves(string userId)
//        {
//            var currentApprove = _identityContext.ApprovesUsers.AsQueryable().Where(x => x.UserId == userId)
//                .Select(s => s.ApproveStepId).ToList();
//            var approves = (from def in _identityContext.ApproveDefnetions.AsQueryable()
//                            from step in _identityContext.ApproveSteps.AsQueryable().Where(x =>
//                                x.LastProgramaticStep == false && x.Deleted == false && x.ApproveOrder == 1 &&
//                                x.ApproveDefId == def.Id && currentApprove.All(w => w != x.Id))
//                            select new ApproveDefinitionDto()
//                            {
//                                Id = step.Id,
//                                ApproveName = def.Id + "-" + def.ApprovName + " / " + def.SystemName
//                            }).ToList();

//            return Json(new { data = approves });
//        }
//        [HttpPost]
//        public IActionResult SaveUserApprove(string userId, string approves)
//        {
//            try
//            {
//                var userApprove = JsonConvert.DeserializeObject<List<int>>(approves ?? string.Empty);
//                if (userApprove != null)
//                {
//                    var sendApproves = new List<ApprovesUsers>();
//                    var maxId = _identityContext.ApprovesUsers.Max(x => x.Id) + 1;
//                    foreach (var approveId in userApprove)
//                    {
//                        var newUserApprove = new ApprovesUsers()
//                        {
//                            Id = maxId,
//                            ApproveStepId = approveId,
//                            UserId = userId
//                        };
//                        sendApproves.Add(newUserApprove);
//                        maxId++;
//                    }
//                    _identityContext.ApprovesUsers.AddRange(sendApproves);
//                    _identityContext.SaveChanges();
//                }
//                return Json(true);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                return Json(false);
//            }
//        }

//        public IActionResult UserApprovesDelete(string userId, int approveId)
//        {
//            var userApproveInDb = _identityContext.ApprovesUsers.FirstOrDefault(x => x.UserId == userId && x.ApproveStepId == approveId);
//            if (userApproveInDb != null)
//            {
//                _identityContext.ApprovesUsers.Remove(userApproveInDb);
//                _identityContext.SaveChanges();
//                return Json(new { success = true });
//            }
//            return Json(new { success = false });
//        }

//        Falg

//        public IActionResult GetFlags(int menuId, int applicationId, string userId)
//        {
//            if (menuId != 0)
//            {
//                var data =
//                    (from menu in _identityContext.Menus.AsQueryable().Where(x => x.ApplicationId == applicationId && x.MenuId == menuId)
//                     from menuFlag in _identityContext.MenuFlags.AsQueryable().Where(x => x.MenuId == menu.MenuId && x.AppId == applicationId)
//                     from userFlag in _identityContext.UserFlags.AsQueryable().Where(x => x.FlagId == menuFlag.FlagId && x.UserId == userId).DefaultIfEmpty()
//                     select new FlagsVm()
//                     {
//                         FlagId = menuFlag.FlagId,
//                         FlagName = menuFlag.FlagName,
//                         IsExist = userFlag != null
//                     }).ToList();
//                return Json(new { data = data.Where(x => x.IsExist == false) });
//            }
//            else
//            {
//                var data =
//                    (from menuFlag in _identityContext.MenuFlags.AsQueryable().Where(x => x.AppId == applicationId)
//                     from userFlag in _identityContext.UserFlags.AsQueryable().Where(x => x.FlagId == menuFlag.FlagId && x.UserId == userId).DefaultIfEmpty()
//                     select new FlagsVm()
//                     {
//                         FlagId = menuFlag.FlagId,
//                         FlagName = menuFlag.FlagName,
//                         IsExist = userFlag != null
//                     }).ToList();
//                return Json(new { data = data.Where(x => x.IsExist == false) });
//            }
//        }

//        [HttpPost]
//        public IActionResult SaveUserFlags(string userId, string flags)
//        {
//            try
//            {
//                var userFlags = JsonConvert.DeserializeObject<List<int>>(flags ?? string.Empty);
//                if (userFlags != null)
//                {
//                    var sendFlags = new List<UserFlags>();
//                    foreach (var flag in userFlags)
//                    {
//                        var userFlag = new UserFlags()
//                        {
//                            FlagId = flag,
//                            UserId = userId,
//                            FlagValue = "1"
//                        };
//                        sendFlags.Add(userFlag);
//                    }
//                    _identityContext.UserFlags.AddRange(sendFlags);
//                    _identityContext.SaveChanges();
//                }
//                return Json(true);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                return Json(false);
//            }
//        }

//        public IActionResult UserFlag(string userId, string userName)
//        {
//            var model = new MenuFlagVm()
//            {
//                UserId = userId,
//                UserName = userName,
//                Applications = _identityContext.Applications.Select(x => new ApplicationDto()
//                {
//                    Id = x.Id,
//                    AppName = x.AppName
//                }).ToList()
//            };
//            return View(model);
//        }


//        public IActionResult ReadFlags(DataSourceLoadOptions loadOptions, string userId)
//        {
//            var flags =
//                (from menu in _identityContext.Menus.AsQueryable()
//                 from menuFlag in _identityContext.MenuFlags.AsQueryable().Where(x => x.MenuId == menu.MenuId)
//                 from userFlag in _identityContext.UserFlags.AsQueryable().Where(x => x.FlagId == menuFlag.FlagId && x.UserId == userId)
//                 select new FlagsVm()
//                 {
//                     MenuName = menu.MenuName,
//                     FlagId = menuFlag.FlagId,
//                     FlagName = menuFlag.FlagName,
//                     ApplicationName = menuFlag.Application.AppName,
//                     UserId = userId,
//                     Description = menuFlag.Description,
//                     FlagValue = userFlag.FlagValue
//                 }).ToList();
//            var data = DataSourceLoader.Load(flags, loadOptions);
//            return Json(data);
//        }

//        [HttpPost]
//        public IActionResult FlagDelete(string userId, int flagId)
//        {
//            var userFlagInDb = _identityContext.UserFlags.FirstOrDefault(x => x.UserId == userId && x.FlagId == flagId);
//            if (userFlagInDb != null)
//            {
//                _identityContext.UserFlags.Remove(userFlagInDb);
//                _identityContext.SaveChanges();
//                return Json(new { success = true });
//            }
//            return Json(new { success = false });
//        }

//        [HttpPost]
//        public IActionResult FlagUpdate(string userId, int flagId, string flagValue)
//        {
//            var userFlagInDb = _identityContext.UserFlags.FirstOrDefault(x => x.UserId == userId && x.FlagId == flagId);
//            if (userFlagInDb != null)
//            {
//                userFlagInDb.FlagValue = flagValue;
//                _identityContext.UserFlags.Update(userFlagInDb);
//                _identityContext.SaveChanges();
//                return Json(new { success = true });
//            }
//            return Json(new { success = false });
//        }


//        Delete

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> UserDelete(TUserDto user)
//        {
//            var currentUserId = User.GetSubjectId();
//            if (user.Id.ToString() == currentUserId)
//            {
//                return RedirectToAction(nameof(UserDelete), user.Id);
//            }
//            else
//            {
//                var result = await _identityContext.DeleteUserAsync(user.Id.ToString(), user);
//                if (result.Succeeded)
//                {
//                    var validations = _identityContext.UserValidations.Where(x => x.UserId == user.Id.ToString()).ToList();
//                    _identityContext.UserValidations.RemoveRange(validations);
//                    await _identityContext.SaveChangesAsync();

//                    var userFlags = _identityContext.UserFlags.Where(x => x.UserId == user.Id.ToString()).ToList();
//                    _identityContext.UserFlags.RemoveRange(userFlags);
//                    await _identityContext.SaveChangesAsync();


//                    var userRoles = _identityContext.UserRoles.Where(x => x.UserId == user.Id.ToString()).ToList();
//                    _identityContext.UserRoles.RemoveRange(userRoles);
//                    await _identityContext.SaveChangesAsync();

//                    var userApprove = _identityContext.ApprovesUsers.Where(x => x.UserId == user.Id.ToString()).ToList();
//                    _identityContext.ApprovesUsers.RemoveRange(userApprove);
//                    await _identityContext.SaveChangesAsync();
//                }
//                return RedirectToAction(nameof(Users));
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> UserDelete(TKey id)
//        {
//            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

//            var user = await _identityService.GetUserAsync(id.ToString());
//            if (user == null) return NotFound();

//            return View(user);
//        }

//        public IActionResult LandingPage()
//        {

//            return View();
//        }



//        Brances


//         ======= Controller methods(revised) =======

//        / <summary>
//        / يعيد جميع الفروع للتطبيق رقم 5 مع علم الـ IsFound حسب صلاحيات الـ userId
//        / </summary>
//        public async Task<IActionResult> GetBranches(int applicationId, string userId)
//        {
//            السماح فقط للتطبيق رقم 5
//            if (applicationId != 5)
//                return Json(new List<BranchesVm>());

//            var branches = await GetMedicalGreenBranches();

//            طبق صلاحيات المستخدم على القائمة
//            if (!string.IsNullOrEmpty(userId))
//            {
//                var validValues = await _identityContext.UserValidations
//                    .Where(x => x.UserId == userId && x.ApplicationId == applicationId)
//                    .Select(x => x.ValidValue)
//                    .ToListAsync();

//                var valuesSet = new HashSet<int>(validValues);

//                branches = branches
//                    .Select(b => new BranchesVm
//                    {
//                        BaseId = b.BaseId,
//                        NameArab = b.NameArab,
//                        IsFound = valuesSet.Contains(b.BaseId)
//                    })
//                    .ToList();
//            }

//            return Json(branches);
//        }

//        / <summary>
//        / يعيد لائحة UserBranches لأنواع مختلفة - الآن async ومخصص للتطبيق 5 (يرجع فروع المستخدَم فقط)
//        / </summary>
//        public async Task<List<UserBranchesDto>> GetUserBranches(int applicationId, string userId)
//        {
//            if (applicationId != 5)
//                return new List<UserBranchesDto>();

//            var branches = await GetMedicalGreenBranches();

//            var dtoList = branches
//                .Select(x => new UserBranchesDto
//                {
//                    BranchId = x.BaseId,
//                    BranchName = x.NameArab,
//                    IsFound = false // سنحدده بعد ذلك إذا كان userId موجود
//                })
//                .ToList();

//            if (!string.IsNullOrEmpty(userId))
//            {
//                var userValidations = await _identityContext.UserValidations
//                    .Where(x => x.UserId == userId && x.ApplicationId == applicationId)
//                    .Select(x => x.ValidValue)
//                    .ToListAsync();

//                var set = new HashSet<int>(userValidations);

//                dtoList = dtoList
//                    .Select(b => new UserBranchesDto
//                    {
//                        BranchId = b.BranchId,
//                        BranchName = b.BranchName,
//                        IsFound = set.Contains(b.BranchId),
//                        ApplicationId = applicationId,
//                        ApplicationName = _identityContext.Applications.Find(applicationId)?.AppName
//                    })
//                    .ToList();
//            }
//            else
//            {
//                إذا لا يوجد userId، ما نعتبر أي فرع مُعطّى للمستخدم
//                dtoList = dtoList
//                    .Select(b => { b.ApplicationId = applicationId; b.ApplicationName = _identityContext.Applications.Find(applicationId)?.AppName; return b; })
//                    .ToList();
//            }

//            إرجاع الفروع المخصصة للمستخدم فقط
//            return dtoList.Where(x => x.IsFound).ToList();
//        }

//        / <summary>
//        / يعيد فروع التطبيق 5 بعد فلتر حسب userId(لو موجود)
//        / </summary>
//        public async Task<List<BranchesVm>> GetBranchesByUserId(int applicationId, string userId)
//        {
//            if (applicationId != 5)
//                return new List<BranchesVm>();

//            var branches = await GetMedicalGreenBranches();

//            if (!string.IsNullOrEmpty(userId))
//            {
//                var validValues = await _identityContext.UserValidations
//                    .Where(x => x.UserId == userId && x.ApplicationId == applicationId)
//                    .Select(x => x.ValidValue)
//                    .ToListAsync();

//                var set = new HashSet<int>(validValues);

//                branches = branches
//                    .Where(b => set.Contains(b.BaseId))
//                    .Select(b => new BranchesVm
//                    {
//                        BaseId = b.BaseId,
//                        NameArab = b.NameArab
//                    })
//                    .ToList();
//            }

//            return branches;
//        }

//        / <summary>
//        / يعيد كل UserBranches عبر كل التطبيقات
//        / ملاحظة: بما أن GetUserBranches(applicationId, userId) الآن يقبل applicationId ويعيد فقط تطبيق 5
//        / فالدورة ستضيف نتائج التطبيق 5 فقط(ما طلبته سابقاً).
//        / </summary>
//        public async Task<List<UserBranchesDto>> GetUserBranches(string userId)
//        {
//            var model = new List<UserBranchesDto>();

//            جلب كل التطبيقات(الأي ديز)
//            var applications = await _identityContext.Applications
//                .Select(x => x.Id)
//                .ToListAsync();

//            foreach (var application in applications)
//            {
//                استدعاء النسخة الـ async من GetUserBranches
//                var branchesList = await GetUserBranches(application, userId);

//                foreach (var item in branchesList)
//                {
//                    model.Add(new UserBranchesDto()
//                    {
//                        ApplicationId = item.ApplicationId,
//                        BranchId = item.BranchId,
//                        BranchName = item.BranchName,
//                        ApplicationName = item.ApplicationName
//                    });
//                }
//            }

//            return model;
//        }

//        / <summary>
//        / يجلب الفروع من قاعدة البيانات المحلية(HubStore)
//        / </summary>
//        private async Task<List<BranchesVm>> GetMedicalGreenBranches()
//        {
//            try
//            {
//                var branches = await _context.Branches
//                    .Where(b => b.BaseId != 0) // عدّل الشرط لو تريد شروط أخرى
//                    .Select(b => new BranchesVm
//                    {
//                        BaseId = b.BaseId,
//                        NameArab = b.Name_Arab
//                    })
//                    .ToListAsync();

//                return branches;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//                return new List<BranchesVm>();
//            }
//        }

//        public IActionResult GetUser()
//        {
//            var users = _identityContext.Users
//                .Select(u => new
//                {
//                    u.Id,
//                    UserName = u.UserName ?? "",
//                    Email = u.Email ?? "",
//                    PhoneNumber = u.PhoneNumber ?? "",
//                    u.UserSerial,
//                    u.EmployeeId,
//                    Active = u.LockoutEnd.GetValueOrDefault() < DateTimeOffset.Now
//                })
//                .ToList();

//            return Json(users);
//        }

//    }
//}
