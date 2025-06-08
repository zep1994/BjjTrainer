using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BjjTrainer_API.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorToCalendarEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "CalendarEvents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_InstructorId",
                table: "CalendarEvents",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_ApplicationUsers_InstructorId",
                table: "CalendarEvents",
                column: "InstructorId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_ApplicationUsers_InstructorId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_InstructorId",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "CalendarEvents");
        }
    }
}
