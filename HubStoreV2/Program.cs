using Core.Domin;
using Core.Services.ItemServices;
using Core.Services.ConversionServices;
using Core.Services.InventoryServices;
using Core.Services.MovementServices;
using Core.Services.OrderServices;
using Core.Services.ReportingServices;
using Core.Services.SettingServices;
using Core.Services;
using Core.Services.AuthServices;

using HubStoreV2.Services.ConversionServices;
using HubStoreV2.Services.InventoryServices;
using Infrastructure.Data;
using Infrastructure.DataSeeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Services.AuditServices;
using Infrastructure.ServicesImpelemention;
using Infrastructure.ServicesImpelemention.AuthServices;
//using Core.Domin.IdentityData;
//using Repository;
//using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add DevExpress Blazor Services
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HubStore")
    )
);

builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Lockout settings
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

// Register Item Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemUnitService, ItemUnitService>();

// Register Conversion Services
builder.Services.AddScoped<IUnitConversionService, UnitConversionService>();
builder.Services.AddScoped<IAuditTrailService, AuditTrailService>();
builder.Services.AddScoped<IAttemptLogService, AttemptLogService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Register Reporting Services
builder.Services.AddScoped<IInventoryReportService, InventoryReportService>();

// Register Inventory Services
builder.Services.AddScoped<Core.Services.InventoryServices.IInventoryCalculationService, HubStoreV2.Services.InventoryServices.InventoryCalculationService>();
builder.Services.AddScoped<IItemBalanceService, ItemBalanceService>();

// Register Branch Service
builder.Services.AddScoped<IBranchService, BranchService>();

// Register System Utility Service
builder.Services.AddScoped<ISystemUtilityService, SystemUtilityService>();

// Register Purchase Order Services
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IStockInService, StockInService>();
builder.Services.AddScoped<ITransferOrderServiceNew, TransferOrderServiceNew>();
builder.Services.AddScoped<IReturnOrderService, ReturnOrderService>();
builder.Services.AddScoped<IDirectPurchaseOrderService, DirectPurchaseOrderService>();
builder.Services.AddScoped<IStockOutReturnService, StockOutReturnService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Register Attachment Services
builder.Services.AddScoped<Core.Services.AttachmentServices.IAttachmentService, HubStoreV2.Services.AttachmentService>();


//builder.Services
//    .AddIdentity<AppUser, UserIdentityRole>()
//    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
//    .AddDefaultTokenProviders();




var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context);

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var adminUsers = await userManager.Users.Where(u => u.IsAdmin).ToListAsync();
    foreach (var adminUser in adminUsers)
    {
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapBlazorHub();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
