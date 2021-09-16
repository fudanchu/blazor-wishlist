using Microsoft.EntityFrameworkCore.Migrations;

namespace Wishlist.Server.Migrations
{
    public partial class AddingRoleTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39c5f152-3eda-4086-a905-73921aecc04b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e984711-48c8-4e71-b746-6de2c5fba225");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a50ceea-57d7-416f-8be5-1e025b376b3f", "4743cd4e-fc5b-456e-928d-cdc3aba8b424", "Basic", "BASIC" },
                    { "1c333557-4dfe-4a10-9400-f5718ca907a6", "b00211e1-5e2e-4194-a319-afdf99a346d3", "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "400d5b1c-b761-4e2b-b0ef-d9909eefc433",
                column: "NickName",
                value: "Dman");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c333557-4dfe-4a10-9400-f5718ca907a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a50ceea-57d7-416f-8be5-1e025b376b3f");

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39c5f152-3eda-4086-a905-73921aecc04b", "35c42484-4ab6-4110-9961-87ee58c9a38c", "Basic", "BASIC" },
                    { "8e984711-48c8-4e71-b746-6de2c5fba225", "ce80ba87-1a49-41bd-a823-e3383ff813e5", "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "400d5b1c-b761-4e2b-b0ef-d9909eefc433",
                columns: new[] { "NickName", "ShortName" },
                values: new object[] { "Dan", "Dman" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aad29937-f673-426b-b338-fc5b75926c37",
                column: "ShortName",
                value: "Ang");
        }
    }
}
