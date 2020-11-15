using Microsoft.EntityFrameworkCore.Migrations;

namespace Radicitus.Raffle.Migrations
{
    public partial class ForeignKeyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_raffle_number_rad_raffle_id",
                schema: "rad",
                table: "raffle_number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_raffle_number_rad_raffle_id",
                schema: "rad",
                table: "raffle_number",
                column: "id",
                principalSchema: "rad",
                principalTable: "rad_raffle",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
