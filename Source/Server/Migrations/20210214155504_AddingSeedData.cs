using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wishlist.Server.Migrations
{
    public partial class AddingSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39c5f152-3eda-4086-a905-73921aecc04b", "35c42484-4ab6-4110-9961-87ee58c9a38c", "Basic", "BASIC" },
                    { "8e984711-48c8-4e71-b746-6de2c5fba225", "ce80ba87-1a49-41bd-a823-e3383ff813e5", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FamilyId", "FirstName", "LastListUpdate", "LastName", "ListNote", "LockoutEnabled", "LockoutEnd", "NickName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PictureBlob", "PictureType", "SecurityStamp", "ShortName", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "400d5b1c-b761-4e2b-b0ef-d9909eefc433", 0, "caf86e69-0a26-4f93-b7f5-b8557cf3c49c", "mrfudanchu@gmail.com", false, 1, "Daniel", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bronson", "Don't need much.  :)", true, null, "Dan", "MRFUDANCHU@GMAIL.COM", "DANNO", "AQAAAAEAACcQAAAAELRLao2z9QK1uTSF9MkS3XFL7kGTJ5peolLcnUgufG9kG8KuRtvypjjKxRL6nPvxUw==", null, false, null, null, "4IBP2DQCF5ZMZZAQ2POAGGYHQMDNGT4M", "Dman", false, "Danno" },
                    { "aad29937-f673-426b-b338-fc5b75926c37", 0, "642aa26e-a0dd-47de-b145-348e8f6845e7", "fourcamps@gmail.com", false, 2, "Angela", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Camp", "", true, null, "Ang", "FOURCAMPS@GMAIL.COM", "ANGIE", "AQAAAAEAACcQAAAAEApbnFa0lPJ/4EP2Uo60kyLy8ZBcFAezRYiU9soysXiGH9YjCxGcVxB1ugbGxU5/1w==", null, false, null, null, "VJNRLNY67OH36WP3OWRTKNXU2TJK3OBN", "Ang", false, "Angie" }
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39c5f152-3eda-4086-a905-73921aecc04b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e984711-48c8-4e71-b746-6de2c5fba225");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "400d5b1c-b761-4e2b-b0ef-d9909eefc433");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aad29937-f673-426b-b338-fc5b75926c37");

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
        }
    }
}
