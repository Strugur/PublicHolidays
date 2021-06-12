using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicHolidays.Data.Migrations
{
    public partial class add_holiday_type_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Holidays",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "HolidayName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HolidayId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayName_Holidays_HolidayId",
                        column: x => x.HolidayId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HolidayName_HolidayId",
                table: "HolidayName",
                column: "HolidayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayName");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Holidays",
                newName: "Name");
        }
    }
}
