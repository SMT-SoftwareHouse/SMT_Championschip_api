using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMTChampionshipAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatenewDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Groups_GroupId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_GroupId",
                table: "Teams");

            migrationBuilder.AlterColumn<bool>(
                name: "Active",
                table: "Matches",
                type: "bit(1)",
                nullable: true,
                oldClrType: typeof(sbyte),
                oldType: "tinyint(1)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<sbyte>(
                name: "Active",
                table: "Matches",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit(1)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GroupId",
                table: "Teams",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Groups_GroupId",
                table: "Teams",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
