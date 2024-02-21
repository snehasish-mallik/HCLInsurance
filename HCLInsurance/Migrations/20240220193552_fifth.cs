using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCLInsurance.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ClaimPercentage",
                table: "claimModels",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimPercentage",
                table: "claimModels");
        }
    }
}
