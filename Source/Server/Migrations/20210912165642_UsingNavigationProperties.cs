using Microsoft.EntityFrameworkCore.Migrations;

namespace Wishlist.Server.Migrations
{
    public partial class UsingNavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31cdd380-9dc1-48a0-a228-3fe7cdd73604");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0b2ea66-c099-47bd-87e2-695068d16d32");

            migrationBuilder.RenameColumn(
                name: "FamilyId",
                table: "AspNetUsers",
                newName: "GroupId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Gifts",
                type: "Money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "eac6f180-d225-4ebc-96ee-a485d46c9227", "98cd20f1-0342-4dd9-83d2-f9084d0452f6", "Basic", "BASIC" },
                    { "a929bf1c-6156-44a8-a993-efa7e3894f57", "8fd207f4-d975-43f1-b5d7-21eeb6a3feec", "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "400d5b1c-b761-4e2b-b0ef-d9909eefc433",
                columns: new[] { "Email", "FirstName", "LastName", "ListNote", "NickName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "joe_blow_tester@gmail.com", "Joe", "Blow", "This is my example list note.  :)", "Jman", "JOE_BLOW_TESTER@GMAIL.COM", "JOEB", "JoeB" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aad29937-f673-426b-b338-fc5b75926c37",
                columns: new[] { "Email", "FirstName", "LastName", "NickName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "jane_doe_tester@gmail.com", "Jane", "Doe", "", "JANE_DOE_TESTER@GMAIL.COM", "JANED", "JaneD" });

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_UserAskingId",
                table: "Gifts",
                column: "UserAskingId");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_UserBuyingId",
                table: "Gifts",
                column: "UserBuyingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_AspNetUsers_UserAskingId",
                table: "Gifts",
                column: "UserAskingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_AspNetUsers_UserBuyingId",
                table: "Gifts",
                column: "UserBuyingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_AspNetUsers_UserAskingId",
                table: "Gifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_AspNetUsers_UserBuyingId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_UserAskingId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_UserBuyingId",
                table: "Gifts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a929bf1c-6156-44a8-a993-efa7e3894f57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eac6f180-d225-4ebc-96ee-a485d46c9227");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "AspNetUsers",
                newName: "FamilyId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Gifts",
                type: "money",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "Money",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31cdd380-9dc1-48a0-a228-3fe7cdd73604", "8a2be00e-dcdf-4279-a2ba-920baf24f923", "Basic", "BASIC" },
                    { "a0b2ea66-c099-47bd-87e2-695068d16d32", "13dc1c75-f33c-441b-be7a-16152113609f", "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "400d5b1c-b761-4e2b-b0ef-d9909eefc433",
                columns: new[] { "Email", "FirstName", "LastName", "ListNote", "NickName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "mrfudanchu@gmail.com", "Daniel", "Bronson", "Don't need much.  :)", "Dman", "MRFUDANCHU@GMAIL.COM", "DANNO", "Danno" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aad29937-f673-426b-b338-fc5b75926c37",
                columns: new[] { "Email", "FirstName", "LastName", "NickName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "fourcamps@gmail.com", "Angela", "Camp", "Ang", "FOURCAMPS@GMAIL.COM", "ANGIE", "Angie" });
        }
    }
}
