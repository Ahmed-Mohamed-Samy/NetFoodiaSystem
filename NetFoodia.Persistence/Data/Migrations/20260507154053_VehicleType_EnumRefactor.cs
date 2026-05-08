using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetFoodia.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class VehicleType_EnumRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VehicleType",
                table: "VolunteerProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            //migrationBuilder.Sql("UPDATE VolunteerProfiles SET VehicleType = '1' WHERE VehicleType = 'Walking'");
            //migrationBuilder.Sql("UPDATE VolunteerProfiles SET VehicleType = '2' WHERE VehicleType = 'Bicycle'");
            //migrationBuilder.Sql("UPDATE VolunteerProfiles SET VehicleType = '3' WHERE VehicleType = 'Motorcycle'");
            //migrationBuilder.Sql("UPDATE VolunteerProfiles SET VehicleType = '4' WHERE VehicleType = 'Car'");
            //migrationBuilder.Sql("UPDATE VolunteerProfiles SET VehicleType = '1' WHERE VehicleType NOT IN ('1', '2', '3', '4')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "VolunteerProfiles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
