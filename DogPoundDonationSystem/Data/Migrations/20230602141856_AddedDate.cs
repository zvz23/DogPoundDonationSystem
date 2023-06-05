using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogPoundDonationSystem.Data.Migrations
{
    public partial class AddedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Donations",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Donations");
        }
    }
}
