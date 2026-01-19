using Core.Domin;
using Core.Services.ItemServices;
using Core.Services.ConversionServices;
using Core.Services.InventoryServices;
using Core.Services.MovementServices;
using Core.Services.OrderServices;

using HubStoreV2.Services.ConversionServices;
using HubStoreV2.Services.InventoryServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Services.AuditServices;
using Infrastructure.ServicesImpelemention;
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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("HubStore")
    )
);

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

// Register Item Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemUnitService, ItemUnitService>();

// Register Conversion Services
builder.Services.AddScoped<IUnitConversionService, UnitConversionService>();
builder.Services.AddScoped<IAuditTrailService, AuditTrailService>();
builder.Services.AddScoped<IAttemptLogService, AttemptLogService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Register Inventory Services

// Register Purchase Order Services
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IStockInService, StockInService>();
builder.Services.AddScoped<ITransferOrderServiceNew, TransferOrderServiceNew>();
builder.Services.AddScoped<IReturnOrderService, ReturnOrderService>();
//builder.Services.AddScoped<IStockOutReturnService, StockOutReturnService>();


//builder.Services
//    .AddIdentity<AppUser, UserIdentityRole>()
//    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
//    .AddDefaultTokenProviders();




var app = builder.Build();

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
