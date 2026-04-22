using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharityKycDetailsTableRemoveFeild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "CharityKycDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "CharityKycDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
