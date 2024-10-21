using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUser3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "AspNetUsers");
        }
    }
}
