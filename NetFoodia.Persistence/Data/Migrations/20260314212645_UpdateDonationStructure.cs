using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDonationStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Donations",
                newName: "FoodType");

            migrationBuilder.RenameColumn(
                name: "ReviewedAt",
                table: "Donations",
                newName: "PickedUpAt");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "Donations",
                newName: "PreparedTime");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Donations",
                newName: "Notes");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Donations",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedAt",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredAt",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationTime",
                table: "Donations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PickupLatitude",
                table: "Donations",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PickupLongitude",
                table: "Donations",
                type: "decimal(18,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "UrgencyScore",
                table: "Donations",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DeliveredAt",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "ExpirationTime",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "UrgencyScore",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "PreparedTime",
                table: "Donations",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "PickedUpAt",
                table: "Donations",
                newName: "ReviewedAt");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Donations",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "FoodType",
                table: "Donations",
                newName: "Title");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Donations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Donations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
