using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogPoundDonationSystem.Data.Migrations
{
    public partial class CascadeOnDeleteDonation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_UserId",
                table: "Donations");

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
