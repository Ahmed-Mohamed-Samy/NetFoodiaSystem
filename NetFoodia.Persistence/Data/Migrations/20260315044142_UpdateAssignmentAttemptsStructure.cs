using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignmentAttemptsStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Outcome",
                table: "AssignmentAttempts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CandidateLoad",
                table: "AssignmentAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "DistanceKm",
                table: "AssignmentAttempts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "AssignmentAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "EtaMinutes",
                table: "AssignmentAttempts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ScoreAtOffer",
                table: "AssignmentAttempts",
                type: "real",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_DonationId",
                table: "AssignmentAttempts",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentAttempts_Donations_DonationId",
                table: "AssignmentAttempts",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentAttempts_Donations_DonationId",
                table: "AssignmentAttempts");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentAttempts_DonationId",
                table: "AssignmentAttempts");

            migrationBuilder.DropColumn(
                name: "CandidateLoad",
                table: "AssignmentAttempts");

            migrationBuilder.DropColumn(
                name: "DistanceKm",
                table: "AssignmentAttempts");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "AssignmentAttempts");

            migrationBuilder.DropColumn(
                name: "EtaMinutes",
                table: "AssignmentAttempts");

            migrationBuilder.DropColumn(
                name: "ScoreAtOffer",
                table: "AssignmentAttempts");

            migrationBuilder.AlterColumn<int>(
                name: "Outcome",
                table: "AssignmentAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
