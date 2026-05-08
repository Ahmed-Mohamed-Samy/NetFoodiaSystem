using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePickupTaskIndex_SlaDueAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PickupTasks_Status_CreatedAt",
                table: "PickupTasks");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTasks_Status_SlaDueAt",
                table: "PickupTasks",
                columns: new[] { "Status", "SlaDueAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PickupTasks_Status_SlaDueAt",
                table: "PickupTasks");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTasks_Status_CreatedAt",
                table: "PickupTasks",
                columns: new[] { "Status", "CreatedAt" });
        }
    }
}
