using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Wishlist.Server.Migrations
{
    public partial class UpdatingGiftIdToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "962f4182-0c75-49cd-a263-94c9d18e1bbb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de8a9058-d46d-4484-9838-a0bb97e61e13");

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Gifts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31cdd380-9dc1-48a0-a228-3fe7cdd73604", "8a2be00e-dcdf-4279-a2ba-920baf24f923", "Basic", "BASIC" },
                    { "a0b2ea66-c099-47bd-87e2-695068d16d32", "13dc1c75-f33c-441b-be7a-16152113609f", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Gifts",
                columns: new[] { "Id", "Cost", "Description", "Name", "Rank", "TimeAdded", "TimeBought", "UserAskingId", "UserBuyingId", "UserSuggestingId", "WebLink" },
                values: new object[,]
                {
                    { 1L, 39999m, "Shiny new wheels!", "New Car", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400d5b1c-b761-4e2b-b0ef-d9909eefc433", null, null, "https://www.tesla.com" },
                    { 2L, 9m, "So warm....", "Socks", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400d5b1c-b761-4e2b-b0ef-d9909eefc433", null, null, null },
                    { 3L, null, "Your best idea!", "Mystery gift", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400d5b1c-b761-4e2b-b0ef-d9909eefc433", null, null, null },
                    { 4L, null, "Something for our cats", "Cat toys", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aad29937-f673-426b-b338-fc5b75926c37", null, null, null },
                    { 5L, null, "The best moves you know!", "Dance moves", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aad29937-f673-426b-b338-fc5b75926c37", null, null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31cdd380-9dc1-48a0-a228-3fe7cdd73604");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0b2ea66-c099-47bd-87e2-695068d16d32");

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Gifts",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Gifts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "de8a9058-d46d-4484-9838-a0bb97e61e13", "06b7f3cc-09ae-4c95-ae39-ea97c971c7b8", "Basic", "BASIC" },
                    { "962f4182-0c75-49cd-a263-94c9d18e1bbb", "9eb1e063-36ce-46d4-a9b3-8868b2c37984", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Gifts",
                columns: new[] { "Id", "Cost", "Description", "Name", "Rank", "TimeAdded", "TimeBought", "UserAskingId", "UserBuyingId", "UserSuggestingId", "WebLink" },
                values: new object[,]
                {
                    { 1, 39999m, "Shiny new wheels!", "New Car", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400d5b1c-b761-4e2b-b0ef-d9909eefc433", null, null, "https://www.tesla.com" },
                    { 2, 9m, "So warm....", "Socks", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400d5b1c-b761-4e2b-b0ef-d9909eefc433", null, null, null },
                    { 3, null, "Your best idea!", "Mystery gift", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400d5b1c-b761-4e2b-b0ef-d9909eefc433", null, null, null },
                    { 4, null, "Something for our cats", "Cat toys", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aad29937-f673-426b-b338-fc5b75926c37", null, null, null },
                    { 5, null, "The best moves you know!", "Dance moves", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "aad29937-f673-426b-b338-fc5b75926c37", null, null, null }
                });
        }
    }
}
