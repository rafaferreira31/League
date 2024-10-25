using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League.Migrations
{
    /// <inheritdoc />
    public partial class ErrorCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClubStatistics");

            migrationBuilder.AddColumn<int>(
                name: "GamesDrawn",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesLost",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesPlayed",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesWon",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoalsConceded",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoalsScored",
                table: "Clubs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Clubs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamesDrawn",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "GamesLost",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "GamesPlayed",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "GamesWon",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "GoalsConceded",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "GoalsScored",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Clubs");

            migrationBuilder.CreateTable(
                name: "ClubStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    GamesDrawn = table.Column<int>(type: "int", nullable: true),
                    GamesLost = table.Column<int>(type: "int", nullable: true),
                    GamesPlayed = table.Column<int>(type: "int", nullable: true),
                    GamesWon = table.Column<int>(type: "int", nullable: true),
                    GoalsConceded = table.Column<int>(type: "int", nullable: true),
                    GoalsScored = table.Column<int>(type: "int", nullable: true),
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
    }
}
