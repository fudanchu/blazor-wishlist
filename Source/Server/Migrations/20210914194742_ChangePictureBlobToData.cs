using Microsoft.EntityFrameworkCore.Migrations;

namespace Wishlist.Server.Migrations
{
    public partial class ChangePictureBlobToData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4085b01f-fd45-40b6-b533-5217a3e0d3c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f997de8-be5a-4021-bb74-df9721e9125f");

            migrationBuilder.RenameColumn(
                name: "PictureBlob",
                table: "AspNetUsers",
                newName: "PictureData");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ce29cca4-8be2-48ba-b6f3-65be475afb6a", "63d1e533-dab8-400d-9f20-a0805c78e70d", "Basic", "BASIC" },
                    { "bc00428e-7dcf-4a58-a03e-d2cd0f659bfd", "dfcb7742-e04f-44e8-9413-45aa6aa4f8a2", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc00428e-7dcf-4a58-a03e-d2cd0f659bfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce29cca4-8be2-48ba-b6f3-65be475afb6a");

            migrationBuilder.RenameColumn(
                name: "PictureData",
                table: "AspNetUsers",
                newName: "PictureBlob");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6f997de8-be5a-4021-bb74-df9721e9125f", "34745a00-acbd-471a-8f33-7ec42a0f36d1", "Basic", "BASIC" },
                    { "4085b01f-fd45-40b6-b533-5217a3e0d3c7", "96b4d566-27e9-4798-b446-9d9d40d7dcc7", "Admin", "ADMIN" }
                });
        }
    }
}
