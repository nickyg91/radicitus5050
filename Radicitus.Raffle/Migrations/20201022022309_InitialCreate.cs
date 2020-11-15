using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Radicitus.Raffle.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "rad");

            migrationBuilder.CreateTable(
                name: "rad_raffle",
                schema: "rad",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    raffle_name = table.Column<string>(maxLength: 128, nullable: true),
                    date_created_utc = table.Column<DateTime>(nullable: false),
                    winner_name = table.Column<string>(maxLength: 128, nullable: true),
                    amount_won = table.Column<decimal>(nullable: false),
                    square_worth_amount = table.Column<decimal>(nullable: false),
                    winning_square = table.Column<int>(nullable: false),
                    start_date_utc = table.Column<DateTime>(nullable: false),
                    end_date_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rad_raffle", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "raffle_number",
                schema: "rad",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(maxLength: 128, nullable: true),
                    raffle_id = table.Column<int>(nullable: false),
                    number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raffle_number", x => x.id);
                    table.ForeignKey(
                        name: "FK_raffle_number_rad_raffle_id",
                        column: x => x.id,
                        principalSchema: "rad",
                        principalTable: "rad_raffle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_raffle_number_rad_raffle_raffle_id",
                        column: x => x.raffle_id,
                        principalSchema: "rad",
                        principalTable: "rad_raffle",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_raffle_number_raffle_id",
                schema: "rad",
                table: "raffle_number",
                column: "raffle_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "raffle_number",
                schema: "rad");

            migrationBuilder.DropTable(
                name: "rad_raffle",
                schema: "rad");
        }
    }
}
