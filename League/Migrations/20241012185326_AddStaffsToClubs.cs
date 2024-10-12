using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace League.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffsToClubs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Staffs_ClubId",
                table: "Staffs",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Clubs_ClubId",
                table: "Staffs",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Clubs_ClubId",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_ClubId",
                table: "Staffs");
        }
    }
}
