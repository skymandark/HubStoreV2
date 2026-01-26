using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalStatuses",
                columns: table => new
                {
                    ApprovalStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalStatuses", x => x.ApprovalStatusId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserSerial = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    PinCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseId = table.Column<int>(type: "int", nullable: false),
                    Name_Arab = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "CostingMethods",
                columns: table => new
                {
                    CostingMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostingMethods", x => x.CostingMethodId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                columns: table => new
                {
                    FiscalYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYears", x => x.FiscalYearId);
                });

            migrationBuilder.CreateTable(
                name: "FlagTypes",
                columns: table => new
                {
                    FlagTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlagTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlagTypes", x => x.FlagTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ImportTypes",
                columns: table => new
                {
                    ImportTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportTypes", x => x.ImportTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ItemCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.ItemCategoryId);
                    table.ForeignKey(
                        name: "FK_ItemCategories_ItemCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    ItemTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.ItemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "MedicineForms",
                columns: table => new
                {
                    MedicineFormId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineForms", x => x.MedicineFormId);
                });

            migrationBuilder.CreateTable(
                name: "MovementTypes",
                columns: table => new
                {
                    MovementTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementTypes", x => x.MovementTypeId);
                });

            migrationBuilder.CreateTable(
                name: "OrderTypes",
                columns: table => new
                {
                    OrderTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypes", x => x.OrderTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "PendingApprovals",
                columns: table => new
                {
                    PendingApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriorityScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingApprovals", x => x.PendingApprovalId);
                });

            migrationBuilder.CreateTable(
                name: "ScientificGroups",
                columns: table => new
                {
                    ScientificGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificGroups", x => x.ScientificGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                });

            migrationBuilder.CreateTable(
                name: "Setting_flag_masters",
                columns: table => new
                {
                    FlagMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlagMasterName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FlagCount = table.Column<int>(type: "int", nullable: true),
                    OneSelection = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting_flag_masters", x => x.FlagMasterId);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentTypes",
                columns: table => new
                {
                    ShipmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentTypes", x => x.ShipmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    SystemSettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.SystemSettingId);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.TaxId);
                });

            migrationBuilder.CreateTable(
                name: "TradeTypes",
                columns: table => new
                {
                    TradeTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeTypes", x => x.TradeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderStatuses",
                columns: table => new
                {
                    TransferOrderStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrderStatuses", x => x.TransferOrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttemptLogs",
                columns: table => new
                {
                    AttemptLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionAttempted = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValidationMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClientIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientDevice = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttemptLogs", x => x.AttemptLogId);
                    table.ForeignKey(
                        name: "FK_AttemptLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExportLogs",
                columns: table => new
                {
                    ExportLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExportType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FiltersJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowCount = table.Column<int>(type: "int", nullable: false),
                    FileReference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportLogs", x => x.ExportLogId);
                    table.ForeignKey(
                        name: "FK_ExportLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PriorityScore = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedViews",
                columns: table => new
                {
                    SavedViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ViewName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FiltersJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColumnsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrderJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedViews", x => x.SavedViewId);
                    table.ForeignKey(
                        name: "FK_SavedViews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalChains",
                columns: table => new
                {
                    ApprovalChainId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    AllowPartialApproval = table.Column<bool>(type: "bit", nullable: false),
                    MinimumApprovalsRequired = table.Column<int>(type: "int", nullable: true),
                    MaximumRejectionsAllowed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalChains", x => x.ApprovalChainId);
                    table.ForeignKey(
                        name: "FK_ApprovalChains_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalChains_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    LogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClientIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClientDevice = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_AuditTrails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditTrails_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainItems",
                columns: table => new
                {
                    MainItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainItems", x => x.MainItemId);
                    table.ForeignKey(
                        name: "FK_MainItems_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Setting_flag_details",
                columns: table => new
                {
                    FlagMasterId = table.Column<int>(type: "int", nullable: false),
                    FlagDetailName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FlagTypeId = table.Column<int>(type: "int", nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: true),
                    FlagValue = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting_flag_details", x => new { x.FlagMasterId, x.FlagDetailName });
                    table.ForeignKey(
                        name: "FK_Setting_flag_details_FlagTypes_FlagTypeId",
                        column: x => x.FlagTypeId,
                        principalTable: "FlagTypes",
                        principalColumn: "FlagTypeId");
                    table.ForeignKey(
                        name: "FK_Setting_flag_details_Setting_flag_masters_FlagMasterId",
                        column: x => x.FlagMasterId,
                        principalTable: "Setting_flag_masters",
                        principalColumn: "FlagMasterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectPurchaseOrderHeaders",
                columns: table => new
                {
                    DirectPurchaseOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ShipmentTypeId = table.Column<int>(type: "int", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreditPayment = table.Column<bool>(type: "bit", nullable: false),
                    PaymentPeriodDays = table.Column<int>(type: "int", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAddedDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectPurchaseOrderHeaders", x => x.DirectPurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_DirectPurchaseOrderHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectPurchaseOrderHeaders_ShipmentTypes_ShipmentTypeId",
                        column: x => x.ShipmentTypeId,
                        principalTable: "ShipmentTypes",
                        principalColumn: "ShipmentTypeId");
                    table.ForeignKey(
                        name: "FK_DirectPurchaseOrderHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectReceiptHeaders",
                columns: table => new
                {
                    DirectReceiptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectReceiptCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ReferenceInvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RemarksArab = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    BranchId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectReceiptHeaders", x => x.DirectReceiptId);
                    table.ForeignKey(
                        name: "FK_DirectReceiptHeaders_ApprovalStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ApprovalStatuses",
                        principalColumn: "ApprovalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectReceiptHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectReceiptHeaders_Branches_BranchId1",
                        column: x => x.BranchId1,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_DirectReceiptHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    MovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovementCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MovementTypeId = table.Column<int>(type: "int", nullable: false),
                    BranchFromId = table.Column<int>(type: "int", nullable: true),
                    BranchToId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalStatusId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPriority = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalQuantityBase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValueCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InternalBarcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.MovementId);
                    table.ForeignKey(
                        name: "FK_Movements_ApprovalStatuses_ApprovalStatusId",
                        column: x => x.ApprovalStatusId,
                        principalTable: "ApprovalStatuses",
                        principalColumn: "ApprovalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movements_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movements_Branches_BranchFromId",
                        column: x => x.BranchFromId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_Movements_Branches_BranchToId",
                        column: x => x.BranchToId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_Movements_MovementTypes_MovementTypeId",
                        column: x => x.MovementTypeId,
                        principalTable: "MovementTypes",
                        principalColumn: "MovementTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movements_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderHeaders",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    BranchStockId = table.Column<int>(type: "int", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TotalBeforeDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    SupplierId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderHeaders", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_Branches_BranchStockId",
                        column: x => x.BranchStockId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_Suppliers_SupplierId1",
                        column: x => x.SupplierId1,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderHeaders",
                columns: table => new
                {
                    ReturnOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ReturnType = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ReturnReasonId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderHeaders", x => x.ReturnOrderId);
                    table.ForeignKey(
                        name: "FK_ReturnOrderHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnOrderHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderHeaders",
                columns: table => new
                {
                    TransferOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferOrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromBranchId = table.Column<int>(type: "int", nullable: false),
                    ToBranchId = table.Column<int>(type: "int", nullable: false),
                    ShipmentTypeId = table.Column<int>(type: "int", nullable: true),
                    TransferOrderStatusId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrderHeaders", x => x.TransferOrderId);
                    table.ForeignKey(
                        name: "FK_TransferOrderHeaders_Branches_FromBranchId",
                        column: x => x.FromBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_TransferOrderHeaders_Branches_ToBranchId",
                        column: x => x.ToBranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferOrderHeaders_ShipmentTypes_ShipmentTypeId",
                        column: x => x.ShipmentTypeId,
                        principalTable: "ShipmentTypes",
                        principalColumn: "ShipmentTypeId");
                    table.ForeignKey(
                        name: "FK_TransferOrderHeaders_TransferOrderStatuses_TransferOrderStatusId",
                        column: x => x.TransferOrderStatusId,
                        principalTable: "TransferOrderStatuses",
                        principalColumn: "TransferOrderStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubItems",
                columns: table => new
                {
                    SubItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainItemId = table.Column<int>(type: "int", nullable: false),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubItems", x => x.SubItemId);
                    table.ForeignKey(
                        name: "FK_SubItems_MainItems_MainItemId",
                        column: x => x.MainItemId,
                        principalTable: "MainItems",
                        principalColumn: "MainItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderTypeId = table.Column<int>(type: "int", nullable: false),
                    RelatedDocumentId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    BranchFromId = table.Column<int>(type: "int", nullable: true),
                    BranchToId = table.Column<int>(type: "int", nullable: true),
                    RequestedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovalStatusId = table.Column<int>(type: "int", nullable: false),
                    IsManualClosed = table.Column<bool>(type: "bit", nullable: false),
                    TotalQuantityBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TotalValueCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriorityFlag = table.Column<int>(type: "int", nullable: false),
                    SLA_DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ClosingReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DirectReceiptHeaderDirectReceiptId = table.Column<int>(type: "int", nullable: true),
                    OrderTypeId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_ApprovalStatuses_ApprovalStatusId",
                        column: x => x.ApprovalStatusId,
                        principalTable: "ApprovalStatuses",
                        principalColumn: "ApprovalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchFromId",
                        column: x => x.BranchFromId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_Orders_Branches_BranchToId",
                        column: x => x.BranchToId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_Orders_DirectReceiptHeaders_DirectReceiptHeaderDirectReceiptId",
                        column: x => x.DirectReceiptHeaderDirectReceiptId,
                        principalTable: "DirectReceiptHeaders",
                        principalColumn: "DirectReceiptId");
                    table.ForeignKey(
                        name: "FK_Orders_OrderTypes_OrderTypeId",
                        column: x => x.OrderTypeId,
                        principalTable: "OrderTypes",
                        principalColumn: "OrderTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_OrderTypes_OrderTypeId1",
                        column: x => x.OrderTypeId1,
                        principalTable: "OrderTypes",
                        principalColumn: "OrderTypeId");
                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "SupplierInvoiceHeaders",
                columns: table => new
                {
                    SupplierInvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    DirectReceiptId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierInvoiceHeaders", x => x.SupplierInvoiceId);
                    table.ForeignKey(
                        name: "FK_SupplierInvoiceHeaders_DirectReceiptHeaders_DirectReceiptId",
                        column: x => x.DirectReceiptId,
                        principalTable: "DirectReceiptHeaders",
                        principalColumn: "DirectReceiptId");
                    table.ForeignKey(
                        name: "FK_SupplierInvoiceHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockOutReturnHeaders",
                columns: table => new
                {
                    StockOutReturnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiptType = table.Column<int>(type: "int", nullable: false),
                    DocCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    BranchStockId = table.Column<int>(type: "int", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAddedDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutReturnHeaders", x => x.StockOutReturnId);
                    table.ForeignKey(
                        name: "FK_StockOutReturnHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOutReturnHeaders_Branches_BranchStockId",
                        column: x => x.BranchStockId,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_StockOutReturnHeaders_ReturnOrderHeaders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrderHeaders",
                        principalColumn: "ReturnOrderId");
                    table.ForeignKey(
                        name: "FK_StockOutReturnHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "StockInHeaders",
                columns: table => new
                {
                    StockInId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    TransferOrderId = table.Column<int>(type: "int", nullable: true),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: true),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    BranchId1 = table.Column<int>(type: "int", nullable: true),
                    SupplierId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInHeaders", x => x.StockInId);
                    table.ForeignKey(
                        name: "FK_StockInHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockInHeaders_Branches_BranchId1",
                        column: x => x.BranchId1,
                        principalTable: "Branches",
                        principalColumn: "BranchId");
                    table.ForeignKey(
                        name: "FK_StockInHeaders_PurchaseOrderHeaders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderId");
                    table.ForeignKey(
                        name: "FK_StockInHeaders_ReturnOrderHeaders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrderHeaders",
                        principalColumn: "ReturnOrderId");
                    table.ForeignKey(
                        name: "FK_StockInHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockInHeaders_Suppliers_SupplierId1",
                        column: x => x.SupplierId1,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_StockInHeaders_TransferOrderHeaders_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrderHeaders",
                        principalColumn: "TransferOrderId");
                });

            migrationBuilder.CreateTable(
                name: "StockOutHeaders",
                columns: table => new
                {
                    StockOutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    TransferOrderId = table.Column<int>(type: "int", nullable: true),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: true),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutHeaders", x => x.StockOutId);
                    table.ForeignKey(
                        name: "FK_StockOutHeaders_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOutHeaders_ReturnOrderHeaders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrderHeaders",
                        principalColumn: "ReturnOrderId");
                    table.ForeignKey(
                        name: "FK_StockOutHeaders_TransferOrderHeaders_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrderHeaders",
                        principalColumn: "TransferOrderId");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameArab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEng = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InternalBarcode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MainUnitCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BaseUnitCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AverageCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    MainItemId = table.Column<int>(type: "int", nullable: false),
                    SubItemId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    ItemTypeId = table.Column<int>(type: "int", nullable: true),
                    ItemCategoryId = table.Column<int>(type: "int", nullable: true),
                    ParentItemId = table.Column<int>(type: "int", nullable: true),
                    IsParent = table.Column<bool>(type: "bit", nullable: false),
                    SellMainPackageId = table.Column<int>(type: "int", nullable: false),
                    SellSubPackageId = table.Column<int>(type: "int", nullable: true),
                    SellSubPackageCount = table.Column<int>(type: "int", nullable: true),
                    BuyMainPackageId = table.Column<int>(type: "int", nullable: false),
                    BuySubPackageId = table.Column<int>(type: "int", nullable: true),
                    BuySubPackageCount = table.Column<int>(type: "int", nullable: true),
                    PackageCount = table.Column<int>(type: "int", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustomerPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BasicPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostWithoutVat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProfitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MainDiscount = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    NoVat = table.Column<bool>(type: "bit", nullable: false),
                    BuyTradeTypeId = table.Column<int>(type: "int", nullable: true),
                    SellTradeTypeId = table.Column<int>(type: "int", nullable: true),
                    StockAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TradeAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VatAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AskQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    SafeQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    MaxQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    MinQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    AverageQuantityPeriodDays = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CoverageDays = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    OrderMinQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    OrderMaxQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    QuotaQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    NoReplenishment = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsNoStock = table.Column<bool>(type: "bit", nullable: false),
                    IsDiscontinued = table.Column<bool>(type: "bit", nullable: false),
                    IsDeficient = table.Column<bool>(type: "bit", nullable: false),
                    RequiresRefrigeration = table.Column<bool>(type: "bit", nullable: false),
                    IsPos = table.Column<bool>(type: "bit", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsSerialized = table.Column<bool>(type: "bit", nullable: false),
                    IsGift = table.Column<bool>(type: "bit", nullable: false),
                    IsPromoted = table.Column<bool>(type: "bit", nullable: false),
                    HasExpiryDate = table.Column<bool>(type: "bit", nullable: false),
                    HasBatchNumber = table.Column<bool>(type: "bit", nullable: false),
                    IsService = table.Column<bool>(type: "bit", nullable: false),
                    IsRawMaterial = table.Column<bool>(type: "bit", nullable: false),
                    IsFinishedGood = table.Column<bool>(type: "bit", nullable: false),
                    OfferStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfferEndedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    BrandId1 = table.Column<int>(type: "int", nullable: true),
                    ImportTypeId = table.Column<int>(type: "int", nullable: true),
                    ItemCategoryId1 = table.Column<int>(type: "int", nullable: true),
                    ItemTypeId1 = table.Column<int>(type: "int", nullable: true),
                    MainItemId1 = table.Column<int>(type: "int", nullable: true),
                    MedicineFormId = table.Column<int>(type: "int", nullable: true),
                    ScientificGroupId = table.Column<int>(type: "int", nullable: true),
                    SubItemId1 = table.Column<int>(type: "int", nullable: true),
                    VendorId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId");
                    table.ForeignKey(
                        name: "FK_Items_Brands_BrandId1",
                        column: x => x.BrandId1,
                        principalTable: "Brands",
                        principalColumn: "BrandId");
                    table.ForeignKey(
                        name: "FK_Items_ImportTypes_ImportTypeId",
                        column: x => x.ImportTypeId,
                        principalTable: "ImportTypes",
                        principalColumn: "ImportTypeId");
                    table.ForeignKey(
                        name: "FK_Items_ItemCategories_ItemCategoryId",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCategoryId");
                    table.ForeignKey(
                        name: "FK_Items_ItemCategories_ItemCategoryId1",
                        column: x => x.ItemCategoryId1,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCategoryId");
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId");
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeId1",
                        column: x => x.ItemTypeId1,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId");
                    table.ForeignKey(
                        name: "FK_Items_Items_ParentItemId",
                        column: x => x.ParentItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_Items_MainItems_MainItemId",
                        column: x => x.MainItemId,
                        principalTable: "MainItems",
                        principalColumn: "MainItemId");
                    table.ForeignKey(
                        name: "FK_Items_MainItems_MainItemId1",
                        column: x => x.MainItemId1,
                        principalTable: "MainItems",
                        principalColumn: "MainItemId");
                    table.ForeignKey(
                        name: "FK_Items_MedicineForms_MedicineFormId",
                        column: x => x.MedicineFormId,
                        principalTable: "MedicineForms",
                        principalColumn: "MedicineFormId");
                    table.ForeignKey(
                        name: "FK_Items_Packages_BuyMainPackageId",
                        column: x => x.BuyMainPackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId");
                    table.ForeignKey(
                        name: "FK_Items_Packages_BuySubPackageId",
                        column: x => x.BuySubPackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId");
                    table.ForeignKey(
                        name: "FK_Items_Packages_SellMainPackageId",
                        column: x => x.SellMainPackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId");
                    table.ForeignKey(
                        name: "FK_Items_Packages_SellSubPackageId",
                        column: x => x.SellSubPackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId");
                    table.ForeignKey(
                        name: "FK_Items_ScientificGroups_ScientificGroupId",
                        column: x => x.ScientificGroupId,
                        principalTable: "ScientificGroups",
                        principalColumn: "ScientificGroupId");
                    table.ForeignKey(
                        name: "FK_Items_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId");
                    table.ForeignKey(
                        name: "FK_Items_SubItems_SubItemId",
                        column: x => x.SubItemId,
                        principalTable: "SubItems",
                        principalColumn: "SubItemId");
                    table.ForeignKey(
                        name: "FK_Items_SubItems_SubItemId1",
                        column: x => x.SubItemId1,
                        principalTable: "SubItems",
                        principalColumn: "SubItemId");
                    table.ForeignKey(
                        name: "FK_Items_TradeTypes_BuyTradeTypeId",
                        column: x => x.BuyTradeTypeId,
                        principalTable: "TradeTypes",
                        principalColumn: "TradeTypeId");
                    table.ForeignKey(
                        name: "FK_Items_TradeTypes_SellTradeTypeId",
                        column: x => x.SellTradeTypeId,
                        principalTable: "TradeTypes",
                        principalColumn: "TradeTypeId");
                    table.ForeignKey(
                        name: "FK_Items_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId");
                    table.ForeignKey(
                        name: "FK_Items_Vendors_VendorId1",
                        column: x => x.VendorId1,
                        principalTable: "Vendors",
                        principalColumn: "VendorId");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalHistories",
                columns: table => new
                {
                    ApprovalHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatusId = table.Column<int>(type: "int", nullable: false),
                    ActionByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionRoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ActionTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsMandatoryNoteProvided = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatusId1 = table.Column<int>(type: "int", nullable: true),
                    DocumentTypeId1 = table.Column<int>(type: "int", nullable: true),
                    MovementId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    PurchaseOrderHeaderPurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    ReturnOrderHeaderReturnOrderId = table.Column<int>(type: "int", nullable: true),
                    StockInHeaderStockInId = table.Column<int>(type: "int", nullable: true),
                    StockOutHeaderStockOutId = table.Column<int>(type: "int", nullable: true),
                    StockOutReturnHeaderStockOutReturnId = table.Column<int>(type: "int", nullable: true),
                    TransferOrderHeaderTransferOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalHistories", x => x.ApprovalHistoryId);
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_ApprovalStatuses_ApprovalStatusId",
                        column: x => x.ApprovalStatusId,
                        principalTable: "ApprovalStatuses",
                        principalColumn: "ApprovalStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_ApprovalStatuses_ApprovalStatusId1",
                        column: x => x.ApprovalStatusId1,
                        principalTable: "ApprovalStatuses",
                        principalColumn: "ApprovalStatusId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_AspNetUsers_ActionByUserId",
                        column: x => x.ActionByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_DocumentTypes_DocumentTypeId1",
                        column: x => x.DocumentTypeId1,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocumentTypeId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "MovementId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_PurchaseOrderHeaders_PurchaseOrderHeaderPurchaseOrderId",
                        column: x => x.PurchaseOrderHeaderPurchaseOrderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_ReturnOrderHeaders_ReturnOrderHeaderReturnOrderId",
                        column: x => x.ReturnOrderHeaderReturnOrderId,
                        principalTable: "ReturnOrderHeaders",
                        principalColumn: "ReturnOrderId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_StockInHeaders_StockInHeaderStockInId",
                        column: x => x.StockInHeaderStockInId,
                        principalTable: "StockInHeaders",
                        principalColumn: "StockInId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_StockOutHeaders_StockOutHeaderStockOutId",
                        column: x => x.StockOutHeaderStockOutId,
                        principalTable: "StockOutHeaders",
                        principalColumn: "StockOutId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_StockOutReturnHeaders_StockOutReturnHeaderStockOutReturnId",
                        column: x => x.StockOutReturnHeaderStockOutReturnId,
                        principalTable: "StockOutReturnHeaders",
                        principalColumn: "StockOutReturnId");
                    table.ForeignKey(
                        name: "FK_ApprovalHistories_TransferOrderHeaders_TransferOrderHeaderTransferOrderId",
                        column: x => x.TransferOrderHeaderTransferOrderId,
                        principalTable: "TransferOrderHeaders",
                        principalColumn: "TransferOrderId");
                });

            migrationBuilder.CreateTable(
                name: "DirectReceiptDetails",
                columns: table => new
                {
                    DirectReceiptDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectReceiptId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BonusQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectReceiptDetails", x => x.DirectReceiptDetailId);
                    table.ForeignKey(
                        name: "FK_DirectReceiptDetails_DirectReceiptHeaders_DirectReceiptId",
                        column: x => x.DirectReceiptId,
                        principalTable: "DirectReceiptHeaders",
                        principalColumn: "DirectReceiptId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectReceiptDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryLayers",
                columns: table => new
                {
                    InventoryLayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantityRemaining = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceDocumentId = table.Column<int>(type: "int", nullable: false),
                    SourceDocumentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryLayers", x => x.InventoryLayerId);
                    table.ForeignKey(
                        name: "FK_InventoryLayers_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryLayers_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemFlags",
                columns: table => new
                {
                    ItemFlagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    FlagId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemFlags", x => x.ItemFlagId);
                    table.ForeignKey(
                        name: "FK_ItemFlags_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackages",
                columns: table => new
                {
                    ItemPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    PackageCount = table.Column<int>(type: "int", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    PackageTransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LevelId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPackages", x => x.ItemPackageId);
                    table.ForeignKey(
                        name: "FK_ItemPackages_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnits",
                columns: table => new
                {
                    ItemUnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConversionToBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ParentUnitId = table.Column<int>(type: "int", nullable: true),
                    IsDefaultForDisplay = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConversionFactor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnits", x => x.ItemUnitId);
                    table.ForeignKey(
                        name: "FK_ItemUnits_ItemUnits_ParentUnitId",
                        column: x => x.ParentUnitId,
                        principalTable: "ItemUnits",
                        principalColumn: "ItemUnitId");
                    table.ForeignKey(
                        name: "FK_ItemUnits_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovementLines",
                columns: table => new
                {
                    MovementLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovementId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QtyInput = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConversionUsedToBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    QtyBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MovementTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementLines", x => x.MovementLineId);
                    table.ForeignKey(
                        name: "FK_MovementLines_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementLines_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementLines_MovementTypes_MovementTypeId",
                        column: x => x.MovementTypeId,
                        principalTable: "MovementTypes",
                        principalColumn: "MovementTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovementLines_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "MovementId");
                });

            migrationBuilder.CreateTable(
                name: "OpeningBalances",
                columns: table => new
                {
                    OpeningBalanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    OpeningQuantityBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningBalances", x => x.OpeningBalanceId);
                    table.ForeignKey(
                        name: "FK_OpeningBalances_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpeningBalances_FiscalYears_FiscalYear",
                        column: x => x.FiscalYear,
                        principalTable: "FiscalYears",
                        principalColumn: "FiscalYearId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpeningBalances_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    OrderLineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    LineNo = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QtyOrdered = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    QtyReceived = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConversionUsedToBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    QtyBaseOrdered = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    QtyBaseReceived = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LineStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InternalBarcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => x.OrderLineId);
                    table.ForeignKey(
                        name: "FK_OrderLines_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierInvoiceDetails",
                columns: table => new
                {
                    SupplierInvoiceDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierInvoiceId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierInvoiceDetails", x => x.SupplierInvoiceDetailId);
                    table.ForeignKey(
                        name: "FK_SupplierInvoiceDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierInvoiceDetails_SupplierInvoiceHeaders_SupplierInvoiceId",
                        column: x => x.SupplierInvoiceId,
                        principalTable: "SupplierInvoiceHeaders",
                        principalColumn: "SupplierInvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderDetails",
                columns: table => new
                {
                    TransferOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrderDetails", x => x.TransferOrderDetailId);
                    table.ForeignKey(
                        name: "FK_TransferOrderDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferOrderDetails_TransferOrderHeaders_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrderHeaders",
                        principalColumn: "TransferOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectPurchaseOrderDetails",
                columns: table => new
                {
                    DirectPurchaseOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectPurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    Serial = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemPackageId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BonusQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MainDiscountPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MainDiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedDiscountPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedDiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemarksArab = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RemarksEng = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectPurchaseOrderDetails", x => x.DirectPurchaseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_DirectPurchaseOrderDetails_DirectPurchaseOrderHeaders_DirectPurchaseOrderId",
                        column: x => x.DirectPurchaseOrderId,
                        principalTable: "DirectPurchaseOrderHeaders",
                        principalColumn: "DirectPurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectPurchaseOrderDetails_ItemPackages_ItemPackageId",
                        column: x => x.ItemPackageId,
                        principalTable: "ItemPackages",
                        principalColumn: "ItemPackageId");
                    table.ForeignKey(
                        name: "FK_DirectPurchaseOrderDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnitHistories",
                columns: table => new
                {
                    ItemUnitHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OldConversionToBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    NewConversionToBase = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ItemUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnitHistories", x => x.ItemUnitHistoryId);
                    table.ForeignKey(
                        name: "FK_ItemUnitHistories_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemUnitHistories_ItemUnits_ItemUnitId",
                        column: x => x.ItemUnitId,
                        principalTable: "ItemUnits",
                        principalColumn: "ItemUnitId");
                    table.ForeignKey(
                        name: "FK_ItemUnitHistories_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    PurchaseOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    OrderedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    RemainingQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ItemId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.PurchaseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_ItemUnits_UnitId",
                        column: x => x.UnitId,
                        principalTable: "ItemUnits",
                        principalColumn: "ItemUnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Items_ItemId1",
                        column: x => x.ItemId1,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrderHeaders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    ReceiptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderLineId = table.Column<int>(type: "int", nullable: false),
                    MovementId = table.Column<int>(type: "int", nullable: false),
                    QuantityReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityBaseReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.ReceiptId);
                    table.ForeignKey(
                        name: "FK_Receipts_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "MovementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Receipts_OrderLines_OrderLineId",
                        column: x => x.OrderLineId,
                        principalTable: "OrderLines",
                        principalColumn: "OrderLineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockOutDetails",
                columns: table => new
                {
                    StockOutDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOutId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransferOrderDetailId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutDetails", x => x.StockOutDetailId);
                    table.ForeignKey(
                        name: "FK_StockOutDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOutDetails_StockOutHeaders_StockOutId",
                        column: x => x.StockOutId,
                        principalTable: "StockOutHeaders",
                        principalColumn: "StockOutId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOutDetails_TransferOrderDetails_TransferOrderDetailId",
                        column: x => x.TransferOrderDetailId,
                        principalTable: "TransferOrderDetails",
                        principalColumn: "TransferOrderDetailId");
                });

            migrationBuilder.CreateTable(
                name: "StockInDetails",
                columns: table => new
                {
                    StockInDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockInId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BonusQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ConsumerPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PurchaseOrderDetailId = table.Column<int>(type: "int", nullable: true),
                    TransferOrderDetailId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    ItemId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInDetails", x => x.StockInDetailId);
                    table.ForeignKey(
                        name: "FK_StockInDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockInDetails_Items_ItemId1",
                        column: x => x.ItemId1,
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_StockInDetails_PurchaseOrderDetails_PurchaseOrderDetailId",
                        column: x => x.PurchaseOrderDetailId,
                        principalTable: "PurchaseOrderDetails",
                        principalColumn: "PurchaseOrderDetailId");
                    table.ForeignKey(
                        name: "FK_StockInDetails_StockInHeaders_StockInId",
                        column: x => x.StockInId,
                        principalTable: "StockInHeaders",
                        principalColumn: "StockInId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockInDetails_TransferOrderDetails_TransferOrderDetailId",
                        column: x => x.TransferOrderDetailId,
                        principalTable: "TransferOrderDetails",
                        principalColumn: "TransferOrderDetailId");
                });

            migrationBuilder.CreateTable(
                name: "ReturnOrderDetails",
                columns: table => new
                {
                    ReturnOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ReturnReasonId = table.Column<int>(type: "int", nullable: true),
                    OriginalStockInDetailId = table.Column<int>(type: "int", nullable: true),
                    OriginalStockOutDetailId = table.Column<int>(type: "int", nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnOrderDetails", x => x.ReturnOrderDetailId);
                    table.ForeignKey(
                        name: "FK_ReturnOrderDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnOrderDetails_ReturnOrderHeaders_ReturnOrderId",
                        column: x => x.ReturnOrderId,
                        principalTable: "ReturnOrderHeaders",
                        principalColumn: "ReturnOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnOrderDetails_StockInDetails_OriginalStockInDetailId",
                        column: x => x.OriginalStockInDetailId,
                        principalTable: "StockInDetails",
                        principalColumn: "StockInDetailId");
                    table.ForeignKey(
                        name: "FK_ReturnOrderDetails_StockOutDetails_OriginalStockOutDetailId",
                        column: x => x.OriginalStockOutDetailId,
                        principalTable: "StockOutDetails",
                        principalColumn: "StockOutDetailId");
                });

            migrationBuilder.CreateTable(
                name: "StockOutReturnDetails",
                columns: table => new
                {
                    StockOutReturnDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockOutReturnId = table.Column<int>(type: "int", nullable: false),
                    LineSerialNumber = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemPackageId = table.Column<int>(type: "int", nullable: true),
                    OrderQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    BonusQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumerPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddedDiscountPercent = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AddedDiscountValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReturnReasonId = table.Column<int>(type: "int", nullable: true),
                    ReturnOrderDetailId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOutReturnDetails", x => x.StockOutReturnDetailId);
                    table.ForeignKey(
                        name: "FK_StockOutReturnDetails_ItemUnits_ItemPackageId",
                        column: x => x.ItemPackageId,
                        principalTable: "ItemUnits",
                        principalColumn: "ItemUnitId");
                    table.ForeignKey(
                        name: "FK_StockOutReturnDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockOutReturnDetails_ReturnOrderDetails_ReturnOrderDetailId",
                        column: x => x.ReturnOrderDetailId,
                        principalTable: "ReturnOrderDetails",
                        principalColumn: "ReturnOrderDetailId");
                    table.ForeignKey(
                        name: "FK_StockOutReturnDetails_StockOutReturnHeaders_StockOutReturnId",
                        column: x => x.StockOutReturnId,
                        principalTable: "StockOutReturnHeaders",
                        principalColumn: "StockOutReturnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalChains_DocumentTypeId",
                table: "ApprovalChains",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalChains_RoleId",
                table: "ApprovalChains",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_ActionByUserId",
                table: "ApprovalHistories",
                column: "ActionByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_ApprovalStatusId",
                table: "ApprovalHistories",
                column: "ApprovalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_ApprovalStatusId1",
                table: "ApprovalHistories",
                column: "ApprovalStatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_DocumentTypeId",
                table: "ApprovalHistories",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_DocumentTypeId1",
                table: "ApprovalHistories",
                column: "DocumentTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_MovementId",
                table: "ApprovalHistories",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_OrderId",
                table: "ApprovalHistories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_PurchaseOrderHeaderPurchaseOrderId",
                table: "ApprovalHistories",
                column: "PurchaseOrderHeaderPurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_ReturnOrderHeaderReturnOrderId",
                table: "ApprovalHistories",
                column: "ReturnOrderHeaderReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_StockInHeaderStockInId",
                table: "ApprovalHistories",
                column: "StockInHeaderStockInId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_StockOutHeaderStockOutId",
                table: "ApprovalHistories",
                column: "StockOutHeaderStockOutId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_StockOutReturnHeaderStockOutReturnId",
                table: "ApprovalHistories",
                column: "StockOutReturnHeaderStockOutReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_TransferOrderHeaderTransferOrderId",
                table: "ApprovalHistories",
                column: "TransferOrderHeaderTransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AttemptLogs_UserId",
                table: "AttemptLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_DocumentTypeId",
                table: "AuditTrails",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_UserId",
                table: "AuditTrails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectPurchaseOrderDetails_DirectPurchaseOrderId",
                table: "DirectPurchaseOrderDetails",
                column: "DirectPurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectPurchaseOrderDetails_ItemId",
                table: "DirectPurchaseOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectPurchaseOrderDetails_ItemPackageId",
                table: "DirectPurchaseOrderDetails",
                column: "ItemPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectPurchaseOrderHeaders_BranchId",
                table: "DirectPurchaseOrderHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectPurchaseOrderHeaders_ShipmentTypeId",
                table: "DirectPurchaseOrderHeaders",
                column: "ShipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectPurchaseOrderHeaders_SupplierId",
                table: "DirectPurchaseOrderHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectReceiptDetails_DirectReceiptId",
                table: "DirectReceiptDetails",
                column: "DirectReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectReceiptDetails_ItemId",
                table: "DirectReceiptDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectReceiptHeaders_BranchId",
                table: "DirectReceiptHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectReceiptHeaders_BranchId1",
                table: "DirectReceiptHeaders",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_DirectReceiptHeaders_StatusId",
                table: "DirectReceiptHeaders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectReceiptHeaders_SupplierId",
                table: "DirectReceiptHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportLogs_UserId",
                table: "ExportLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLayers_BranchId",
                table: "InventoryLayers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLayers_ItemId",
                table: "InventoryLayers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_ParentCategoryId",
                table: "ItemCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemFlags_ItemId",
                table: "ItemFlags",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackages_ItemId",
                table: "ItemPackages",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackages_PackageId",
                table: "ItemPackages",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BranchId",
                table: "Items",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId",
                table: "Items",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BrandId1",
                table: "Items",
                column: "BrandId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BuyMainPackageId",
                table: "Items",
                column: "BuyMainPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BuySubPackageId",
                table: "Items",
                column: "BuySubPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BuyTradeTypeId",
                table: "Items",
                column: "BuyTradeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ImportTypeId",
                table: "Items",
                column: "ImportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemCategoryId",
                table: "Items",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemCategoryId1",
                table: "Items",
                column: "ItemCategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId1",
                table: "Items",
                column: "ItemTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_MainItemId",
                table: "Items",
                column: "MainItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_MainItemId1",
                table: "Items",
                column: "MainItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_MedicineFormId",
                table: "Items",
                column: "MedicineFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentItemId",
                table: "Items",
                column: "ParentItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ScientificGroupId",
                table: "Items",
                column: "ScientificGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SectionId",
                table: "Items",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SellMainPackageId",
                table: "Items",
                column: "SellMainPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SellSubPackageId",
                table: "Items",
                column: "SellSubPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SellTradeTypeId",
                table: "Items",
                column: "SellTradeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SubItemId",
                table: "Items",
                column: "SubItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SubItemId1",
                table: "Items",
                column: "SubItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Items_VendorId",
                table: "Items",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_VendorId1",
                table: "Items",
                column: "VendorId1");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitHistories_ChangedByUserId",
                table: "ItemUnitHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitHistories_ItemId",
                table: "ItemUnitHistories",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitHistories_ItemUnitId",
                table: "ItemUnitHistories",
                column: "ItemUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_ItemId",
                table: "ItemUnits",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_ParentUnitId",
                table: "ItemUnits",
                column: "ParentUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MainItems_SectionId",
                table: "MainItems",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementLines_BranchId",
                table: "MovementLines",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementLines_ItemId",
                table: "MovementLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementLines_MovementId",
                table: "MovementLines",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_MovementLines_MovementTypeId",
                table: "MovementLines",
                column: "MovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_ApprovalStatusId",
                table: "Movements",
                column: "ApprovalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_BranchFromId",
                table: "Movements",
                column: "BranchFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_BranchToId",
                table: "Movements",
                column: "BranchToId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_CreatedByUserId",
                table: "Movements",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_MovementTypeId",
                table: "Movements",
                column: "MovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_SupplierId",
                table: "Movements",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_BranchId",
                table: "OpeningBalances",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_FiscalYear",
                table: "OpeningBalances",
                column: "FiscalYear");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_ItemId",
                table: "OpeningBalances",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_ItemId",
                table: "OrderLines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_OrderId",
                table: "OrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApprovalStatusId",
                table: "Orders",
                column: "ApprovalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchFromId",
                table: "Orders",
                column: "BranchFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchToId",
                table: "Orders",
                column: "BranchToId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DirectReceiptHeaderDirectReceiptId",
                table: "Orders",
                column: "DirectReceiptHeaderDirectReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTypeId1",
                table: "Orders",
                column: "OrderTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequestedByUserId",
                table: "Orders",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierId",
                table: "Orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ItemId",
                table: "PurchaseOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ItemId1",
                table: "PurchaseOrderDetails",
                column: "ItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderId",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_UnitId",
                table: "PurchaseOrderDetails",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_BranchId",
                table: "PurchaseOrderHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_BranchStockId",
                table: "PurchaseOrderHeaders",
                column: "BranchStockId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_SupplierId",
                table: "PurchaseOrderHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_SupplierId1",
                table: "PurchaseOrderHeaders",
                column: "SupplierId1");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_MovementId",
                table: "Receipts",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_OrderLineId",
                table: "Receipts",
                column: "OrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderDetails_ItemId",
                table: "ReturnOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderDetails_OriginalStockInDetailId",
                table: "ReturnOrderDetails",
                column: "OriginalStockInDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderDetails_OriginalStockOutDetailId",
                table: "ReturnOrderDetails",
                column: "OriginalStockOutDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderDetails_ReturnOrderId",
                table: "ReturnOrderDetails",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderHeaders_BranchId",
                table: "ReturnOrderHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnOrderHeaders_SupplierId",
                table: "ReturnOrderHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedViews_UserId",
                table: "SavedViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_flag_details_FlagTypeId",
                table: "Setting_flag_details",
                column: "FlagTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetails_ItemId",
                table: "StockInDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetails_ItemId1",
                table: "StockInDetails",
                column: "ItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetails_PurchaseOrderDetailId",
                table: "StockInDetails",
                column: "PurchaseOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetails_StockInId",
                table: "StockInDetails",
                column: "StockInId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInDetails_TransferOrderDetailId",
                table: "StockInDetails",
                column: "TransferOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_BranchId",
                table: "StockInHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_BranchId1",
                table: "StockInHeaders",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_PurchaseOrderId",
                table: "StockInHeaders",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_ReturnOrderId",
                table: "StockInHeaders",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_SupplierId",
                table: "StockInHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_SupplierId1",
                table: "StockInHeaders",
                column: "SupplierId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockInHeaders_TransferOrderId",
                table: "StockInHeaders",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutDetails_ItemId",
                table: "StockOutDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutDetails_StockOutId",
                table: "StockOutDetails",
                column: "StockOutId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutDetails_TransferOrderDetailId",
                table: "StockOutDetails",
                column: "TransferOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutHeaders_BranchId",
                table: "StockOutHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutHeaders_ReturnOrderId",
                table: "StockOutHeaders",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutHeaders_TransferOrderId",
                table: "StockOutHeaders",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnDetails_ItemId",
                table: "StockOutReturnDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnDetails_ItemPackageId",
                table: "StockOutReturnDetails",
                column: "ItemPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnDetails_ReturnOrderDetailId",
                table: "StockOutReturnDetails",
                column: "ReturnOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnDetails_StockOutReturnId",
                table: "StockOutReturnDetails",
                column: "StockOutReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnHeaders_BranchId",
                table: "StockOutReturnHeaders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnHeaders_BranchStockId",
                table: "StockOutReturnHeaders",
                column: "BranchStockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnHeaders_ReturnOrderId",
                table: "StockOutReturnHeaders",
                column: "ReturnOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockOutReturnHeaders_SupplierId",
                table: "StockOutReturnHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SubItems_MainItemId",
                table: "SubItems",
                column: "MainItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoiceDetails_ItemId",
                table: "SupplierInvoiceDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoiceDetails_SupplierInvoiceId",
                table: "SupplierInvoiceDetails",
                column: "SupplierInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoiceHeaders_DirectReceiptId",
                table: "SupplierInvoiceHeaders",
                column: "DirectReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInvoiceHeaders_SupplierId",
                table: "SupplierInvoiceHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderDetails_ItemId",
                table: "TransferOrderDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderDetails_TransferOrderId",
                table: "TransferOrderDetails",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderHeaders_FromBranchId",
                table: "TransferOrderHeaders",
                column: "FromBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderHeaders_ShipmentTypeId",
                table: "TransferOrderHeaders",
                column: "ShipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderHeaders_ToBranchId",
                table: "TransferOrderHeaders",
                column: "ToBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderHeaders_TransferOrderStatusId",
                table: "TransferOrderHeaders",
                column: "TransferOrderStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalChains");

            migrationBuilder.DropTable(
                name: "ApprovalHistories");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "AttemptLogs");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "CostingMethods");

            migrationBuilder.DropTable(
                name: "DirectPurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "DirectReceiptDetails");

            migrationBuilder.DropTable(
                name: "ExportLogs");

            migrationBuilder.DropTable(
                name: "InventoryLayers");

            migrationBuilder.DropTable(
                name: "ItemFlags");

            migrationBuilder.DropTable(
                name: "ItemUnitHistories");

            migrationBuilder.DropTable(
                name: "MovementLines");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OpeningBalances");

            migrationBuilder.DropTable(
                name: "PendingApprovals");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropTable(
                name: "SavedViews");

            migrationBuilder.DropTable(
                name: "Setting_flag_details");

            migrationBuilder.DropTable(
                name: "StockOutReturnDetails");

            migrationBuilder.DropTable(
                name: "SupplierInvoiceDetails");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "DirectPurchaseOrderHeaders");

            migrationBuilder.DropTable(
                name: "ItemPackages");

            migrationBuilder.DropTable(
                name: "FiscalYears");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "FlagTypes");

            migrationBuilder.DropTable(
                name: "Setting_flag_masters");

            migrationBuilder.DropTable(
                name: "ReturnOrderDetails");

            migrationBuilder.DropTable(
                name: "StockOutReturnHeaders");

            migrationBuilder.DropTable(
                name: "SupplierInvoiceHeaders");

            migrationBuilder.DropTable(
                name: "MovementTypes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "StockInDetails");

            migrationBuilder.DropTable(
                name: "StockOutDetails");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DirectReceiptHeaders");

            migrationBuilder.DropTable(
                name: "OrderTypes");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "StockInHeaders");

            migrationBuilder.DropTable(
                name: "StockOutHeaders");

            migrationBuilder.DropTable(
                name: "TransferOrderDetails");

            migrationBuilder.DropTable(
                name: "ApprovalStatuses");

            migrationBuilder.DropTable(
                name: "ItemUnits");

            migrationBuilder.DropTable(
                name: "PurchaseOrderHeaders");

            migrationBuilder.DropTable(
                name: "ReturnOrderHeaders");

            migrationBuilder.DropTable(
                name: "TransferOrderHeaders");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "ShipmentTypes");

            migrationBuilder.DropTable(
                name: "TransferOrderStatuses");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "ImportTypes");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "MedicineForms");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "ScientificGroups");

            migrationBuilder.DropTable(
                name: "SubItems");

            migrationBuilder.DropTable(
                name: "TradeTypes");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "MainItems");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
