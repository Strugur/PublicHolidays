using Microsoft.EntityFrameworkCore.Migrations;

namespace PublicHolidays.Data.Migrations
{
    public partial class with_dataannotation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Countries_CountryId",
                table: "Holidays");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Countries_CountryId",
                table: "Holidays",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Countries_CountryId",
                table: "Holidays");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Holidays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Countries_CountryId",
                table: "Holidays",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
