using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class AddDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTimeUTC",
                table: "TouristRoutes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTimeUTC",
                table: "TouristRoutes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("2430bf64-fd56-460c-8b75-da0a1d1cd74c"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 3, 31, 21, 12, 18, 457, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("39996f34-013c-4fc6-b1b3-0c1036c47169"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 3, 31, 21, 12, 18, 457, DateTimeKind.Utc).AddTicks(4150));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("3ecbcd92-a9e0-45f7-9b29-e03272cb0862"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 3, 31, 21, 12, 18, 457, DateTimeKind.Utc).AddTicks(1090));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("88cf89b9-e4b5-4b42-a5bf-622bd3039601"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 3, 31, 21, 12, 18, 457, DateTimeKind.Utc).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("a1fd0bee-0afc-4586-96c8-f46b7c99d2a0"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 3, 31, 21, 12, 18, 456, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("fb6d4f10-79ed-4aff-a915-4ce29dc9c7e1"),
                column: "CreateTimeUTC",
                value: new DateTime(2020, 3, 31, 21, 12, 18, 428, DateTimeKind.Utc).AddTicks(1120));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTimeUTC",
                table: "TouristRoutes");

            migrationBuilder.DropColumn(
                name: "UpdateTimeUTC",
                table: "TouristRoutes");
        }
    }
}
