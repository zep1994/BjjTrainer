using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BjjTrainer_API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMoveId1FromUserTrainingGoalMove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTrainingGoalMoves_Moves_MoveId1",
                table: "UserTrainingGoalMoves");

            migrationBuilder.DropIndex(
                name: "IX_UserTrainingGoalMoves_MoveId1",
                table: "UserTrainingGoalMoves");

            migrationBuilder.DropColumn(
                name: "MoveId1",
                table: "UserTrainingGoalMoves");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoveId1",
                table: "UserTrainingGoalMoves",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingGoalMoves_MoveId1",
                table: "UserTrainingGoalMoves",
                column: "MoveId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrainingGoalMoves_Moves_MoveId1",
                table: "UserTrainingGoalMoves",
                column: "MoveId1",
                principalTable: "Moves",
                principalColumn: "Id");
        }
    }
}
