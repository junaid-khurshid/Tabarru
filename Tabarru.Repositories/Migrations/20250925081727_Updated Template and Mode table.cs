using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTemplateandModetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Campaigns_CampaignId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_CampaignId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Templates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CampaignId",
                table: "Templates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CampaignId",
                table: "Templates",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Campaigns_CampaignId",
                table: "Templates",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
