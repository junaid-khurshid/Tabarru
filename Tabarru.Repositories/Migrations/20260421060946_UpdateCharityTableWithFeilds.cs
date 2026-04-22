using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharityTableWithFeilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharityName",
                table: "Charities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Charities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharityName",
                table: "Charities");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Charities");
        }
    }
}
