using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League.Migrations
{
    /// <inheritdoc />
    public partial class ClubStatitics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    GamesPlayed = table.Column<int>(type: "int", nullable: true),
                    GamesWon = table.Column<int>(type: "int", nullable: true),
                    GamesDrawn = table.Column<int>(type: "int", nullable: true),
                    GamesLost = table.Column<int>(type: "int", nullable: true),
                    GoalsScored = table.Column<int>(type: "int", nullable: true),
                    GoalsConceded = table.Column<int>(type: "int", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubStatistics_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubStatistics_ClubId",
                table: "ClubStatistics",
                column: "ClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubStatistics");
        }
    }
}
