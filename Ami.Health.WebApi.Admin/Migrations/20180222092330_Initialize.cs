using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ami.Health.WebApi.Admin.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Department = table.Column<string>(maxLength: 50, nullable: false),
                    Designation = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 128, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logger",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<Guid>(nullable: false),
                    LogInfo = table.Column<string>(maxLength: 500, nullable: false),
                    LoggedDate = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountStatus = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 400, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    DOB = table.Column<DateTime>(type: "DATE", nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    IsFirstTimeLogin = table.Column<bool>(nullable: false),
                    NRC = table.Column<string>(maxLength: 30, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 256, nullable: false),
                    PasswordSalt = table.Column<string>(maxLength: 10, nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: true),
                    PolicyNo = table.Column<string>(maxLength: 30, nullable: false),
                    SecurityQuestion1 = table.Column<string>(maxLength: 50, nullable: true),
                    SecurityQuestion2 = table.Column<string>(maxLength: 50, nullable: true),
                    SecurityQuestion3 = table.Column<string>(maxLength: 50, nullable: true),
                    SecurityQuestion4 = table.Column<string>(maxLength: 50, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    UserCode = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserCode",
                table: "Users",
                column: "UserCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Logger");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
