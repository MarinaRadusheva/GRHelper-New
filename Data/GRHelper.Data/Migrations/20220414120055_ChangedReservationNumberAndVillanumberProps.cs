using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GRHelper.Data.Migrations
{
    public partial class ChangedReservationNumberAndVillanumberProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VillaNumber",
                table: "Villas",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "ReservationNumber",
                table: "Reservations",
                newName: "Number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Villas",
                newName: "VillaNumber");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Reservations",
                newName: "ReservationNumber");
        }
    }
}
