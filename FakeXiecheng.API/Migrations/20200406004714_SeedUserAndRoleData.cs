using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class SeedUserAndRoleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "308660dc-ae51-480f-824d-7dca6714c3e2", "2754c676-248b-46d2-b8f6-67e948d41c02", "Admin", "ADMIN" },
                    { "2aaf05a4-57ce-4a20-a997-06fe9f0d3809", "1382c66f-8035-4cc5-983f-2dd8e1bd8b9b", "Author", "AUTHOR" },
                    { "3dfa307e-d498-4ceb-a9c5-f0d55d103093", "bad19b09-12df-422e-b71c-dc7e012d9416", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90184155-dee0-40c9-bb1e-b5ed07afc04e", 0, null, "f528f928-365f-47bb-8b13-91159422dbe8", "admin@fakexiecheng.com", true, false, null, "ADMIN@FAKEXIECHENG.COM", "ADMIN@FAKEXIECHENG.COM", "AQAAAAEAACcQAAAAEL5xKlFzGnBXaHj62f0XRtkvrxJZjGQ61v3NZ6f55Eb+KlYoI9dExfg3td1fZaNuxA==", null, false, "", false, "admin@fakexiecheng.com" });

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("2430bf64-fd56-460c-8b75-da0a1d1cd74c"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 47, 13, 859, DateTimeKind.Utc).AddTicks(4628));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47169"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 47, 13, 859, DateTimeKind.Utc).AddTicks(5187));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("3ecbcd92-a9e0-45f7-9b29-e03272cb0862"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 47, 13, 859, DateTimeKind.Utc).AddTicks(2534));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("88cf89b9-e4b5-4b42-a5bf-622bd3039601"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 47, 13, 859, DateTimeKind.Utc).AddTicks(3500));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("a1fd0bee-0afc-4586-96c8-f46b7c99d2a0"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 47, 13, 858, DateTimeKind.Utc).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("fb6d4f10-79ed-4aff-a915-4ce29dc9c7e1"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 47, 13, 833, DateTimeKind.Utc).AddTicks(8519));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "ApplicationUserId" },
                values: new object[] { "90184155-dee0-40c9-bb1e-b5ed07afc04e", "308660dc-ae51-480f-824d-7dca6714c3e2", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2aaf05a4-57ce-4a20-a997-06fe9f0d3809");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3dfa307e-d498-4ceb-a9c5-f0d55d103093");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "90184155-dee0-40c9-bb1e-b5ed07afc04e", "308660dc-ae51-480f-824d-7dca6714c3e2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e");

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("2430bf64-fd56-460c-8b75-da0a1d1cd74c"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 38, 7, 815, DateTimeKind.Utc).AddTicks(875));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47169"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 38, 7, 815, DateTimeKind.Utc).AddTicks(1436));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("3ecbcd92-a9e0-45f7-9b29-e03272cb0862"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 38, 7, 814, DateTimeKind.Utc).AddTicks(8752));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("88cf89b9-e4b5-4b42-a5bf-622bd3039601"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 38, 7, 814, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("a1fd0bee-0afc-4586-96c8-f46b7c99d2a0"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 38, 7, 814, DateTimeKind.Utc).AddTicks(2260));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("fb6d4f10-79ed-4aff-a915-4ce29dc9c7e1"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 4, 6, 0, 38, 7, 789, DateTimeKind.Utc).AddTicks(2755));
        }
    }
}
