using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Follows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FollowerUserId",
                table: "Follows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FollowingUserId",
                table: "Follows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerUserId",
                table: "Follows",
                column: "FollowerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowingUserId",
                table: "Follows",
                column: "FollowingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_FollowerUserId",
                table: "Follows",
                column: "FollowerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Users_FollowingUserId",
                table: "Follows",
                column: "FollowingUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_FollowerUserId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Users_FollowingUserId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowerUserId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowingUserId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "FollowerUserId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "FollowingUserId",
                table: "Follows");
        }
    }
}
