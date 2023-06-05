using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogPoundDonationSystem.Data.Migrations
{
    public partial class RemoveNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonationId",
                table: "DonationItem",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DonationId",
                table: "DonationItem",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
