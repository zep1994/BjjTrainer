using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BjjTrainer_API.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipFieldsToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipEndDate",
                table: "ApplicationUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembershipPlan",
                table: "ApplicationUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MembershipStartDate",
                table: "ApplicationUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembershipStatus",
                table: "ApplicationUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "ApplicationUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSubscriptionId",
                table: "ApplicationUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipEndDate",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "MembershipPlan",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "MembershipStartDate",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "MembershipStatus",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "StripeSubscriptionId",
                table: "ApplicationUsers");
        }
    }
}
