using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCharityAdminPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CharityAdminProfiles",
                table: "CharityAdminProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CharityAdminProfiles_UserId",
                table: "CharityAdminProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CharityAdminProfiles");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Charities",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Charities",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharityAdminProfiles",
                table: "CharityAdminProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CharityAdminProfiles",
                table: "CharityAdminProfiles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CharityAdminProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Charities",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Charities",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharityAdminProfiles",
                table: "CharityAdminProfiles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CharityAdminProfiles_UserId",
                table: "CharityAdminProfiles",
                column: "UserId",
                unique: true);
        }
    }
}
