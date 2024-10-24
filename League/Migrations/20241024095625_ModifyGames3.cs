using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League.Migrations
{
    /// <inheritdoc />
    public partial class ModifyGames3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitedAssignedCards",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "VisitorAssignedCards",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitedAssignedCards",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitorAssignedCards",
                table: "Games",
                type: "int",
                nullable: true);
        }
    }
}
