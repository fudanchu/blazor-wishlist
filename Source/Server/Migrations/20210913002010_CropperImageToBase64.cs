using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wishlist.Server.Migrations
{
    public partial class CropperImageToBase64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a929bf1c-6156-44a8-a993-efa7e3894f57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eac6f180-d225-4ebc-96ee-a485d46c9227");

            migrationBuilder.AlterColumn<string>(
                name: "PictureBlob",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldMaxLength: 1048576,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6f997de8-be5a-4021-bb74-df9721e9125f", "34745a00-acbd-471a-8f33-7ec42a0f36d1", "Basic", "BASIC" },
                    { "4085b01f-fd45-40b6-b533-5217a3e0d3c7", "96b4d566-27e9-4798-b446-9d9d40d7dcc7", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4085b01f-fd45-40b6-b533-5217a3e0d3c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f997de8-be5a-4021-bb74-df9721e9125f");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PictureBlob",
                table: "AspNetUsers",
                type: "bytea",
                maxLength: 1048576,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "eac6f180-d225-4ebc-96ee-a485d46c9227", "98cd20f1-0342-4dd9-83d2-f9084d0452f6", "Basic", "BASIC" },
                    { "a929bf1c-6156-44a8-a993-efa7e3894f57", "8fd207f4-d975-43f1-b5d7-21eeb6a3feec", "Admin", "ADMIN" }
                });
        }
    }
}
