using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wishlist.Server.Migrations
{
    public partial class ForgotPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c333557-4dfe-4a10-9400-f5718ca907a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a50ceea-57d7-416f-8be5-1e025b376b3f");

            migrationBuilder.AddColumn<Guid>(
                name: "PasswordResetCode",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTimestamp",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "de8a9058-d46d-4484-9838-a0bb97e61e13", "06b7f3cc-09ae-4c95-ae39-ea97c971c7b8", "Basic", "BASIC" },
                    { "962f4182-0c75-49cd-a263-94c9d18e1bbb", "9eb1e063-36ce-46d4-a9b3-8868b2c37984", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "962f4182-0c75-49cd-a263-94c9d18e1bbb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de8a9058-d46d-4484-9838-a0bb97e61e13");

            migrationBuilder.DropColumn(
                name: "PasswordResetCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordResetTimestamp",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a50ceea-57d7-416f-8be5-1e025b376b3f", "4743cd4e-fc5b-456e-928d-cdc3aba8b424", "Basic", "BASIC" },
                    { "1c333557-4dfe-4a10-9400-f5718ca907a6", "b00211e1-5e2e-4194-a319-afdf99a346d3", "Admin", "ADMIN" }
                });
        }
    }
}
