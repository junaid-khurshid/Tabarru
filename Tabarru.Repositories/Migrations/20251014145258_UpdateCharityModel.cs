using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharityKycDetails_Charity_CharityId",
                table: "CharityKycDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Charity",
                table: "Charity");

            migrationBuilder.RenameTable(
                name: "Charity",
                newName: "Charities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Charities",
                table: "Charities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharityKycDetails_Charities_CharityId",
                table: "CharityKycDetails",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharityKycDetails_Charities_CharityId",
                table: "CharityKycDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Charities",
                table: "Charities");

            migrationBuilder.RenameTable(
                name: "Charities",
                newName: "Charity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Charity",
                table: "Charity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharityKycDetails_Charity_CharityId",
                table: "CharityKycDetails",
                column: "CharityId",
                principalTable: "Charity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
