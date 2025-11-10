using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabarru.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "RecurringPaymentDetails",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "PaymentDetails",
                newName: "VendorType");

            migrationBuilder.AddColumn<string>(
                name: "CharityId",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TemplateId",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharityId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "PaymentDetails");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "RecurringPaymentDetails",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "VendorType",
                table: "PaymentDetails",
                newName: "PaymentId");
        }
    }
}
