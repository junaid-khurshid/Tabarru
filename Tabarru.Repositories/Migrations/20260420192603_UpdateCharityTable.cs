using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharityPhoneNumber",
                table: "Charities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Charities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharityPhoneNumber",
                table: "Charities");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Charities");
        }
    }
}
