using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TtWork.ProjectName.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Parameters = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ReturnValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ImpersonatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    ImpersonatorTenantId = table.Column<int>(type: "int", nullable: true),
                    CustomData = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpBackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobType = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    JobArgs = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(type: "smallint", nullable: false),
                    NextTryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAbandoned = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpBackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChangeSets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpersonatorTenantId = table.Column<int>(type: "int", nullable: true),
                    ImpersonatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChangeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLanguageTexts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LanguageName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 67108864, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLanguageTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(type: "tinyint", nullable: false),
                    UserIds = table.Column<string>(type: "nvarchar(max)", maxLength: 131072, nullable: true),
                    ExcludedUserIds = table.Column<string>(type: "nvarchar(max)", maxLength: 131072, nullable: true),
                    TenantIds = table.Column<string>(type: "nvarchar(max)", maxLength: 131072, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpNotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    NotificationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpNotificationSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnitRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OUType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenantNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    NotificationName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenantNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserLinkId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenancyName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserNameOrEmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClientIpAddress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Result = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TenantNotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadImgUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Town = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Jf = table.Column<int>(type: "int", nullable: false),
                    FromClient = table.Column<int>(type: "int", nullable: false),
                    SignInTokenExpireTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignInToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthenticationSource = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EmailConfirmationCode = table.Column<string>(type: "nvarchar(328)", maxLength: 328, nullable: true),
                    PasswordResetCode = table.Column<string>(type: "nvarchar(328)", maxLength: 328, nullable: true),
                    LockoutEndDateUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    IsLockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsPhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsTwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedEmailAddress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpUsers_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    WebhookUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Webhooks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity_LuckDraws",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: true),
                    MaxDrawTimes = table.Column<int>(type: "int", nullable: false),
                    PrizeTotal = table.Column<int>(type: "int", nullable: false),
                    PrizeCount = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    HtmlContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatetimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatetimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PrizeExpiredTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckCodes = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_LuckDraws", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity_PointActivities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    HtmlContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    HelpPerDay = table.Column<int>(type: "int", nullable: false),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatetimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatetimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStart = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    TimeEnd = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    MinPoint = table.Column<int>(type: "int", nullable: false),
                    MaxPoint = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_PointActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity_UserPointLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AfterPoints = table.Column<int>(type: "int", nullable: false),
                    BeforePoints = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_UserPointLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity_UserPrizes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrizeId = table.Column<long>(type: "bigint", nullable: false),
                    LuckDrawId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    QrUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CheckTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CheckUserId = table.Column<long>(type: "bigint", nullable: true),
                    CheckPhoneNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ExpiredTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_UserPrizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity_VotePlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HtmlContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IsUserUpload = table.Column<bool>(type: "bit", nullable: false),
                    UploadStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UploadEndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoteStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoteEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_VotePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App_Apps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_Apps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit_AuditFlows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    NodesMaxIndex = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit_AuditFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feature_AbpFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    DateTimeExpired = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature_AbpFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QA_QAPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HtmlContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    HelpPerDay = table.Column<int>(type: "int", nullable: false),
                    AnswerPerDay = table.Column<int>(type: "int", nullable: false),
                    SharePoints = table.Column<int>(type: "int", nullable: false),
                    QuestionCount = table.Column<int>(type: "int", nullable: false),
                    DatetimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatetimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStart = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    TimeEnd = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QA_QAPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QA_UserQuestionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    RightCount = table.Column<int>(type: "int", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true),
                    QuestionItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShareFrom = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    SpendTime = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QA_UserQuestionLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_CmsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CmsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Places",
                columns: table => new
                {
                    Uid = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    City = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    District = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Places", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "T_Poster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BgImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BgWidth = table.Column<int>(type: "int", nullable: false),
                    BgHeight = table.Column<int>(type: "int", nullable: false),
                    MainImageDetail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    QrImageDetail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    HeadImageDetail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TitleTextDetail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DescTextDetail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Poster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_QrDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    EventKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Params = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    QrImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_QrDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_TenPayNotify",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    out_trade_no = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    result_code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    fee_type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    return_code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    total_fee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mch_id = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    cash_fee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    openid = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    transaction_id = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    sign = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    bank_type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    appid = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    time_end = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    trade_type = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    nonce_str = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    is_subscribe = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_TenPayNotify", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_ExecutionErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutionPointerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_ExecutionErrors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    ExecutionPointerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SubscribeAsOf = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExternalWorkerId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExternalTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_WorkflowDefinitions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Steps = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_WorkflowDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_Workflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NextExecution = table.Column<long>(type: "bigint", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateUserIdentityName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_Workflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timeline_TimelineCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeline_TimelineCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicEntityProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityFullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DynamicPropertyId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicEntityProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicEntityProperties_AbpDynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "AbpDynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicPropertyValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DynamicPropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicPropertyValues_AbpDynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "AbpDynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditionId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpFeatures_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityChanges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeType = table.Column<byte>(type: "tinyint", nullable: false),
                    EntityChangeSetId = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    EntityTypeFullName = table.Column<string>(type: "nvarchar(192)", maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId",
                        column: x => x.EntityChangeSetId,
                        principalTable: "AbpEntityChangeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_OrganizationUnitDetails",
                columns: table => new
                {
                    ProjectNameOrganizationUnitId = table.Column<long>(type: "bigint", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LogoImgUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    HeadmanRealName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    HeadmanPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    City = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    District = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Lng = table.Column<double>(type: "float", nullable: true),
                    Lat = table.Column<double>(type: "float", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OrganizationUnitDetails", x => x.ProjectNameOrganizationUnitId);
                    table.ForeignKey(
                        name: "FK_T_OrganizationUnitDetails_AbpOrganizationUnits_ProjectNameOrganizationUnitId",
                        column: x => x.ProjectNameOrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    IsStatic = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpRoles_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpSettings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpTenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenancyName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpTenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpEditions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "AbpEditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbpTenants_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserClaims_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserLogins_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserOrganizationUnits_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserTokens_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdSwiper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    SwiperType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdSwiper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdSwiper_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdSwiper_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Audit_AuditUserLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AuditName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AuditStatus = table.Column<int>(type: "int", nullable: true),
                    Expired = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit_AuditUserLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Audit_AuditUserLogs_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaborUnion_Craftsman",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    Detail_Realname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_Sex = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Detail_HeadImgUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Detail_Birthday = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_NativePlace = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_Nation = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_PoliticsStatus = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_EducationBackground = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_SkillLevel = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_TimeOfStartWork = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Detail_TimeOfStartWorkLocal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Detail_Address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_WorkUnit = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_WorkTitle = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_PersonalResume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail_MainAchievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail_MainEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail_OpinionsOfWorkUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail_OpinionsOfRecommandUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail_OpinionsOfLeadingGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Audit = table.Column<int>(type: "int", nullable: true),
                    AuditStatus = table.Column<int>(type: "int", nullable: true),
                    RejectText = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    RedpacketRecived = table.Column<bool>(type: "bit", nullable: false),
                    RedpacketRecivedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Redpacket = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityStamp = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborUnion_Craftsman", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaborUnion_Craftsman_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaborUnion_CraftsmanRecommends",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    RedpacketRecived = table.Column<bool>(type: "bit", nullable: false),
                    RedpacketRecivedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Redpacket = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityStamp = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Detail_Realname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_Sex = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Detail_Age = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Detail_PoliticsStatus = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_Address = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_WorkUnit = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_WorkTitle = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Detail_PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_Desc = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    RejectText = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Audit = table.Column<int>(type: "int", nullable: true),
                    AuditStatus = table.Column<int>(type: "int", nullable: true),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborUnion_CraftsmanRecommends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaborUnion_CraftsmanRecommends_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadImgUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Nickname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    HostType = table.Column<int>(type: "int", nullable: false),
                    FromClient = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ReplyContent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Comments_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_Comments_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_Comments_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_OrganizationApply",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Detail_Desc = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Detail_LogoImgUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Detail_Address = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Detail_HeadmanRealName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_HeadmanPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Detail_Province = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Detail_City = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Detail_CityId = table.Column<int>(type: "int", nullable: true),
                    Detail_District = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Detail_Lng = table.Column<double>(type: "float", nullable: true),
                    Detail_Lat = table.Column<double>(type: "float", nullable: true),
                    Detail_ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Audit = table.Column<int>(type: "int", nullable: true),
                    AuditStatus = table.Column<int>(type: "int", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OrganizationApply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_OrganizationApply_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_OrganizationApply_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_OrganizationApply_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_OrganizationEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OrganizationEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_OrganizationEvents_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_PayOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Body = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    BillNo = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    OpenId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PayType = table.Column<int>(type: "int", nullable: false),
                    IsSuccessPay = table.Column<bool>(type: "bit", nullable: false),
                    SuccessPayTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRefund = table.Column<bool>(type: "bit", nullable: false),
                    RefundTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundComplateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundPrice = table.Column<int>(type: "int", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ExtensionData = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FromClient = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    FromUserId = table.Column<long>(type: "bigint", nullable: true),
                    Commission = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PayOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_PayOrders_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_PayOrders_AbpUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_RefundLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillNo = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    SuccessTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Audit = table.Column<int>(type: "int", nullable: true),
                    AuditStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RefundLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_RefundLogs_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_RefundLogs_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_UserEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    FromUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_UserEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_UserEvents_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_UserEvents_AbpUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_WechatUserinfos",
                columns: table => new
                {
                    openid = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    unionid = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    nickname = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    headimgurl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    city = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    province = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    country = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    sex = table.Column<int>(type: "int", nullable: false),
                    FromClient = table.Column<int>(type: "int", nullable: false),
                    appid = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    appName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WechatUserinfos", x => new { x.openid, x.TenantId });
                    table.ForeignKey(
                        name: "FK_T_WechatUserinfos_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpWebhookSendAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpWebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "AbpWebhookEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity_LuckDrawPrizes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    LuckDrawId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_LuckDrawPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_LuckDrawPrizes_Activity_LuckDraws_LuckDrawId",
                        column: x => x.LuckDrawId,
                        principalTable: "Activity_LuckDraws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity_PointActivityUserLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<long>(type: "bigint", nullable: false),
                    HelperNum = table.Column<int>(type: "int", nullable: false),
                    Point = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    SharedFrom = table.Column<long>(type: "bigint", nullable: true),
                    openid = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    nickname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    headimgurl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_PointActivityUserLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_PointActivityUserLogs_Activity_PointActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity_PointActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity_VoteItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    VotePlanId = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Audit = table.Column<int>(type: "int", nullable: true),
                    AuditStatus = table.Column<int>(type: "int", nullable: true),
                    RejectText = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_VoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_VoteItems_Activity_VotePlans_VotePlanId",
                        column: x => x.VotePlanId,
                        principalTable: "Activity_VotePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Audit_AuditNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit_AuditNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Audit_AuditNodes_Audit_AuditFlows_AuditFlowId",
                        column: x => x.AuditFlowId,
                        principalTable: "Audit_AuditFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QA_QAQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerIndex = table.Column<int>(type: "int", nullable: false),
                    AnswerList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QA_QAQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QA_QAQuestions_QA_QAPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "QA_QAPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CmsContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TitleImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    LinkType = table.Column<int>(type: "int", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CmsContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_CmsContents_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CmsContents_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CmsContents_T_CmsCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "T_CmsCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_ExecutionPointers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    SleepUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersistenceData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventPublished = table.Column<bool>(type: "bit", nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StepName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContextItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredecessorId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_ExecutionPointers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_WorkFlow_ExecutionPointers_T_WorkFlow_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "T_WorkFlow_Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timeline_TimelineEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    TitleImageUrl = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HtmlContext = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatetimeStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatetimeEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Audit = table.Column<int>(type: "int", nullable: true),
                    AuditStatus = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeline_TimelineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timeline_TimelineEvents_Timeline_TimelineCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Timeline_TimelineCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpDynamicEntityPropertyValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicEntityPropertyId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpDynamicEntityPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpDynamicEntityPropertyValues_AbpDynamicEntityProperties_DynamicEntityPropertyId",
                        column: x => x.DynamicEntityPropertyId,
                        principalTable: "AbpDynamicEntityProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpEntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityChangeId = table.Column<long>(type: "bigint", nullable: false),
                    NewValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    PropertyTypeFullName = table.Column<string>(type: "nvarchar(192)", maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    NewValueHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalValueHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "AbpEntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpPermissions_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleClaims",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CmsContentEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CmsContentId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CmsContentEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_CmsContentEvents_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CmsContentEvents_T_CmsContents_CmsContentId",
                        column: x => x.CmsContentId,
                        principalTable: "T_CmsContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkFlow_ExtensionAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExecutionPointerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AttributeKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkFlow_ExtensionAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_WorkFlow_ExtensionAttributes_T_WorkFlow_ExecutionPointers_ExecutionPointerId",
                        column: x => x.ExecutionPointerId,
                        principalTable: "T_WorkFlow_ExecutionPointers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Timeline_TimelineFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<long>(type: "bigint", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<long>(type: "bigint", nullable: true),
                    ExtensionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeline_TimelineFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timeline_TimelineFiles_Timeline_TimelineEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "Timeline_TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionDuration",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_ExecutionTime",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuditLogs_TenantId_UserId",
                table: "AbpAuditLogs",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityProperties_DynamicPropertyId",
                table: "AbpDynamicEntityProperties",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityProperties_EntityFullName_DynamicPropertyId_TenantId",
                table: "AbpDynamicEntityProperties",
                columns: new[] { "EntityFullName", "DynamicPropertyId", "TenantId" },
                unique: true,
                filter: "[EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicEntityPropertyValues_DynamicEntityPropertyId",
                table: "AbpDynamicEntityPropertyValues",
                column: "DynamicEntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicProperties_PropertyName_TenantId",
                table: "AbpDynamicProperties",
                columns: new[] { "PropertyName", "TenantId" },
                unique: true,
                filter: "[PropertyName] IS NOT NULL AND [TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AbpDynamicPropertyValues_DynamicPropertyId",
                table: "AbpDynamicPropertyValues",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityChangeSetId",
                table: "AbpEntityChanges",
                column: "EntityChangeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChanges_EntityTypeFullName_EntityId",
                table: "AbpEntityChanges",
                columns: new[] { "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_CreationTime",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_Reason",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "Reason" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityChangeSets_TenantId_UserId",
                table: "AbpEntityChangeSets",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_EditionId_Name",
                table: "AbpFeatures",
                columns: new[] { "EditionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpFeatures_TenantId_Name",
                table: "AbpFeatures",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguages_TenantId_Name",
                table: "AbpLanguages",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key",
                table: "AbpLanguageTexts",
                columns: new[] { "TenantId", "Source", "LanguageName", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId",
                table: "AbpNotificationSubscriptions",
                columns: new[] { "TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_TenantId_RoleId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_TenantId_Code",
                table: "AbpOrganizationUnits",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_RoleId",
                table: "AbpPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_TenantId_Name",
                table: "AbpPermissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_UserId",
                table: "AbpPermissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_TenantId_ClaimType",
                table: "AbpRoleClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_CreatorUserId",
                table: "AbpRoles",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_DeleterUserId",
                table: "AbpRoles",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_LastModifierUserId",
                table: "AbpRoles",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_TenantId_NormalizedName",
                table: "AbpRoles",
                columns: new[] { "TenantId", "NormalizedName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_TenantId_Name_UserId",
                table: "AbpSettings",
                columns: new[] { "TenantId", "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_UserId",
                table: "AbpSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenantNotifications_TenantId",
                table: "AbpTenantNotifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_CreatorUserId",
                table: "AbpTenants",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_DeleterUserId",
                table: "AbpTenants",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_EditionId",
                table: "AbpTenants",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_LastModifierUserId",
                table: "AbpTenants",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_TenancyName",
                table: "AbpTenants",
                column: "TenancyName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_EmailAddress",
                table: "AbpUserAccounts",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_EmailAddress",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "EmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserId",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_TenantId_UserName",
                table: "AbpUserAccounts",
                columns: new[] { "TenantId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserAccounts_UserName",
                table: "AbpUserAccounts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_TenantId_ClaimType",
                table: "AbpUserClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "AbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result",
                table: "AbpUserLoginAttempts",
                columns: new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLoginAttempts_UserId_TenantId",
                table: "AbpUserLoginAttempts",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_TenantId_UserId",
                table: "AbpUserLogins",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_UserId",
                table: "AbpUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserNotifications_UserId_State_CreationTime",
                table: "AbpUserNotifications",
                columns: new[] { "UserId", "State", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_TenantId_UserId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_UserId",
                table: "AbpUserOrganizationUnits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_RoleId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_TenantId_UserId",
                table: "AbpUserRoles",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_UserId",
                table: "AbpUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_CreatorUserId",
                table: "AbpUsers",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_DeleterUserId",
                table: "AbpUsers",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_LastModifierUserId",
                table: "AbpUsers",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedEmailAddress",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedEmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TenantId_NormalizedUserName",
                table: "AbpUsers",
                columns: new[] { "TenantId", "NormalizedUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_TenantId_UserId",
                table: "AbpUserTokens",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserTokens_UserId",
                table: "AbpUserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpWebhookSendAttempts_WebhookEventId",
                table: "AbpWebhookSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_LuckDrawPrizes_LuckDrawId",
                table: "Activity_LuckDrawPrizes",
                column: "LuckDrawId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_PointActivityUserLogs_ActivityId",
                table: "Activity_PointActivityUserLogs",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_VoteItems_VotePlanId",
                table: "Activity_VoteItems",
                column: "VotePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AdSwiper_CreatorUserId",
                table: "AdSwiper",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdSwiper_LastModifierUserId",
                table: "AdSwiper",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_AuditNodes_AuditFlowId",
                table: "Audit_AuditNodes",
                column: "AuditFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_AuditUserLogs_CreatorUserId",
                table: "Audit_AuditUserLogs",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LaborUnion_Craftsman_CreatorUserId",
                table: "LaborUnion_Craftsman",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LaborUnion_CraftsmanRecommends_CreatorUserId",
                table: "LaborUnion_CraftsmanRecommends",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QA_QAQuestions_PlanId",
                table: "QA_QAQuestions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CmsContentEvents_CmsContentId",
                table: "T_CmsContentEvents",
                column: "CmsContentId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CmsContentEvents_CreatorUserId",
                table: "T_CmsContentEvents",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CmsContents_CategoryId",
                table: "T_CmsContents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CmsContents_CreatorUserId",
                table: "T_CmsContents",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CmsContents_LastModifierUserId",
                table: "T_CmsContents",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Comments_CreatorUserId",
                table: "T_Comments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Comments_DeleterUserId",
                table: "T_Comments",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Comments_LastModifierUserId",
                table: "T_Comments",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_OrganizationApply_CreatorUserId",
                table: "T_OrganizationApply",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_OrganizationApply_LastModifierUserId",
                table: "T_OrganizationApply",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_OrganizationApply_OrganizationUnitId",
                table: "T_OrganizationApply",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_T_OrganizationEvents_CreatorUserId",
                table: "T_OrganizationEvents",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PayOrders_CreatorUserId",
                table: "T_PayOrders",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PayOrders_FromUserId",
                table: "T_PayOrders",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_RefundLogs_CreatorUserId",
                table: "T_RefundLogs",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_RefundLogs_LastModifierUserId",
                table: "T_RefundLogs",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_UserEvents_CreatorUserId",
                table: "T_UserEvents",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_UserEvents_FromUserId",
                table: "T_UserEvents",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WechatUserinfos_CreatorUserId",
                table: "T_WechatUserinfos",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Events_EventName_EventKey",
                table: "T_WorkFlow_Events",
                columns: new[] { "EventName", "EventKey" });

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Events_EventTime",
                table: "T_WorkFlow_Events",
                column: "EventTime");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Events_IsProcessed",
                table: "T_WorkFlow_Events",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_ExecutionPointers_WorkflowId",
                table: "T_WorkFlow_ExecutionPointers",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_ExtensionAttributes_ExecutionPointerId",
                table: "T_WorkFlow_ExtensionAttributes",
                column: "ExecutionPointerId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Subscriptions_EventKey",
                table: "T_WorkFlow_Subscriptions",
                column: "EventKey");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Subscriptions_EventName",
                table: "T_WorkFlow_Subscriptions",
                column: "EventName");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Workflows_Id",
                table: "T_WorkFlow_Workflows",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkFlow_Workflows_NextExecution",
                table: "T_WorkFlow_Workflows",
                column: "NextExecution");

            migrationBuilder.CreateIndex(
                name: "IX_Timeline_TimelineEvents_CategoryId",
                table: "Timeline_TimelineEvents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Timeline_TimelineFiles_EventId",
                table: "Timeline_TimelineFiles",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuditLogs");

            migrationBuilder.DropTable(
                name: "AbpBackgroundJobs");

            migrationBuilder.DropTable(
                name: "AbpDynamicEntityPropertyValues");

            migrationBuilder.DropTable(
                name: "AbpDynamicPropertyValues");

            migrationBuilder.DropTable(
                name: "AbpEntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "AbpFeatures");

            migrationBuilder.DropTable(
                name: "AbpLanguages");

            migrationBuilder.DropTable(
                name: "AbpLanguageTexts");

            migrationBuilder.DropTable(
                name: "AbpNotifications");

            migrationBuilder.DropTable(
                name: "AbpNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AbpPermissions");

            migrationBuilder.DropTable(
                name: "AbpRoleClaims");

            migrationBuilder.DropTable(
                name: "AbpSettings");

            migrationBuilder.DropTable(
                name: "AbpTenantNotifications");

            migrationBuilder.DropTable(
                name: "AbpTenants");

            migrationBuilder.DropTable(
                name: "AbpUserAccounts");

            migrationBuilder.DropTable(
                name: "AbpUserClaims");

            migrationBuilder.DropTable(
                name: "AbpUserLoginAttempts");

            migrationBuilder.DropTable(
                name: "AbpUserLogins");

            migrationBuilder.DropTable(
                name: "AbpUserNotifications");

            migrationBuilder.DropTable(
                name: "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpUserRoles");

            migrationBuilder.DropTable(
                name: "AbpUserTokens");

            migrationBuilder.DropTable(
                name: "AbpWebhookSendAttempts");

            migrationBuilder.DropTable(
                name: "AbpWebhookSubscriptions");

            migrationBuilder.DropTable(
                name: "Activity_LuckDrawPrizes");

            migrationBuilder.DropTable(
                name: "Activity_PointActivityUserLogs");

            migrationBuilder.DropTable(
                name: "Activity_UserPointLogs");

            migrationBuilder.DropTable(
                name: "Activity_UserPrizes");

            migrationBuilder.DropTable(
                name: "Activity_VoteItems");

            migrationBuilder.DropTable(
                name: "AdSwiper");

            migrationBuilder.DropTable(
                name: "App_Apps");

            migrationBuilder.DropTable(
                name: "Audit_AuditNodes");

            migrationBuilder.DropTable(
                name: "Audit_AuditUserLogs");

            migrationBuilder.DropTable(
                name: "Feature_AbpFeatures");

            migrationBuilder.DropTable(
                name: "LaborUnion_Craftsman");

            migrationBuilder.DropTable(
                name: "LaborUnion_CraftsmanRecommends");

            migrationBuilder.DropTable(
                name: "QA_QAQuestions");

            migrationBuilder.DropTable(
                name: "QA_UserQuestionLogs");

            migrationBuilder.DropTable(
                name: "T_CmsContentEvents");

            migrationBuilder.DropTable(
                name: "T_Comments");

            migrationBuilder.DropTable(
                name: "T_OrganizationApply");

            migrationBuilder.DropTable(
                name: "T_OrganizationEvents");

            migrationBuilder.DropTable(
                name: "T_OrganizationUnitDetails");

            migrationBuilder.DropTable(
                name: "T_PayOrders");

            migrationBuilder.DropTable(
                name: "T_Places");

            migrationBuilder.DropTable(
                name: "T_Poster");

            migrationBuilder.DropTable(
                name: "T_QrDetails");

            migrationBuilder.DropTable(
                name: "T_RefundLogs");

            migrationBuilder.DropTable(
                name: "T_TenPayNotify");

            migrationBuilder.DropTable(
                name: "T_UserEvents");

            migrationBuilder.DropTable(
                name: "T_WechatUserinfos");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_Events");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_ExecutionErrors");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_ExtensionAttributes");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_Subscriptions");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_WorkflowDefinitions");

            migrationBuilder.DropTable(
                name: "Timeline_TimelineFiles");

            migrationBuilder.DropTable(
                name: "AbpDynamicEntityProperties");

            migrationBuilder.DropTable(
                name: "AbpEntityChanges");

            migrationBuilder.DropTable(
                name: "AbpRoles");

            migrationBuilder.DropTable(
                name: "AbpEditions");

            migrationBuilder.DropTable(
                name: "AbpWebhookEvents");

            migrationBuilder.DropTable(
                name: "Activity_LuckDraws");

            migrationBuilder.DropTable(
                name: "Activity_PointActivities");

            migrationBuilder.DropTable(
                name: "Activity_VotePlans");

            migrationBuilder.DropTable(
                name: "Audit_AuditFlows");

            migrationBuilder.DropTable(
                name: "QA_QAPlans");

            migrationBuilder.DropTable(
                name: "T_CmsContents");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_ExecutionPointers");

            migrationBuilder.DropTable(
                name: "Timeline_TimelineEvents");

            migrationBuilder.DropTable(
                name: "AbpDynamicProperties");

            migrationBuilder.DropTable(
                name: "AbpEntityChangeSets");

            migrationBuilder.DropTable(
                name: "AbpUsers");

            migrationBuilder.DropTable(
                name: "T_CmsCategories");

            migrationBuilder.DropTable(
                name: "T_WorkFlow_Workflows");

            migrationBuilder.DropTable(
                name: "Timeline_TimelineCategories");
        }
    }
}
