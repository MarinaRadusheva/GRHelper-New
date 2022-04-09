using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GRHelper.Data.Migrations
{
    public partial class AddedReservationNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationNumber",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationNumber",
                table: "Reservations");
        }
    }
}
