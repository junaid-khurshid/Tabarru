using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTemplateAndCampaignFeilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "Campaigns",
                newName: "isStudentForm");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "Campaigns",
                newName: "isMembershipForm");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryColor",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryColor",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShapeId",
                table: "Templates",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryColor",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "SecondaryColor",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "ShapeId",
                table: "Templates");

            migrationBuilder.RenameColumn(
                name: "isStudentForm",
                table: "Campaigns",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "isMembershipForm",
                table: "Campaigns",
                newName: "IsDefault");
        }
    }
}
