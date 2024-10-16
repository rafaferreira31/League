using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League.Migrations
{
    /// <inheritdoc />
    public partial class ModifyGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitedClub",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "VisitorClub",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "VisitedClubId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisitedClubName",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitorClubId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VisitorClubName",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitedClubId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "VisitedClubName",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "VisitorClubId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "VisitorClubName",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "VisitedClub",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VisitorClub",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
