using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickupTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationId = table.Column<int>(type: "int", nullable: false),
                    CharityId = table.Column<int>(type: "int", nullable: false),
                    AssignedVolunteerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SlaDueAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupTasks_Charities_CharityId",
                        column: x => x.CharityId,
                        principalTable: "Charities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickupTasks_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickupTasks_VolunteerProfiles_AssignedVolunteerId",
                        column: x => x.AssignedVolunteerId,
                        principalTable: "VolunteerProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickupTaskId = table.Column<int>(type: "int", nullable: false),
                    VolunteerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OfferedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Response = table.Column<int>(type: "int", nullable: false),
                    Outcome = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentAttempts_PickupTasks_PickupTaskId",
                        column: x => x.PickupTaskId,
                        principalTable: "PickupTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentAttempts_VolunteerProfiles_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "VolunteerProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_PickupTaskId",
                table: "AssignmentAttempts",
                column: "PickupTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_VolunteerId",
                table: "AssignmentAttempts",
                column: "VolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTasks_AssignedVolunteerId",
                table: "PickupTasks",
                column: "AssignedVolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTasks_CharityId",
                table: "PickupTasks",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupTasks_DonationId",
                table: "PickupTasks",
                column: "DonationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentAttempts");

            migrationBuilder.DropTable(
                name: "PickupTasks");
        }
    }
}
