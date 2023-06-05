using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogPoundDonationSystem.Data.Migrations
{
    public partial class NullableDonationItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonationItem_Donations_DonationId",
                table: "DonationItem");

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
        }
    }
}
