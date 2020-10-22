using Microsoft.EntityFrameworkCore.Migrations;

namespace Radicitus.Raffle.Migrations
{
    public partial class RaffleNullableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "winning_square",
                schema: "rad",
                table: "rad_raffle",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_won",
                schema: "rad",
                table: "rad_raffle",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "winning_square",
                schema: "rad",
                table: "rad_raffle",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "amount_won",
                schema: "rad",
                table: "rad_raffle",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
