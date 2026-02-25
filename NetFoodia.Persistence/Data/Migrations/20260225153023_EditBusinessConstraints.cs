using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditBusinessConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DonorProfile_BusinessRules",
                table: "DonorProfiles");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DonorProfile_BusinessRules",
                table: "DonorProfiles",
                sql: "([IsBusiness] = 1 AND [BusinessType] IS NOT NULL) OR ([IsBusiness] = 0 AND [BusinessType] IS NULL AND [IsVerified] = 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_DonorProfile_BusinessRules",
                table: "DonorProfiles");

            migrationBuilder.AddCheckConstraint(
                name: "CK_DonorProfile_BusinessRules",
                table: "DonorProfiles",
                sql: "([IsBusiness] = 1 AND [BusinessType] IS NOT NULL) OR ([IsBusiness] = 0 AND [BusinessType] IS NULL AND [IsVerified] = 0)");
        }
    }
}
