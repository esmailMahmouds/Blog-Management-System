using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Status",
                table: "Posts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreateDate",
                table: "Posts",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Status_CreateDate",
                table: "Posts",
                columns: new[] { "Status", "CreateDate" });


            migrationBuilder.CreateIndex(
                name: "IX_Likes_PostId_UserId",
                table: "Likes",
                columns: new[] { "PostId", "UserId" },
                unique: true);


            migrationBuilder.CreateIndex(
                name: "IX_Ratings_PostId_UserId",
                table: "Ratings",
                columns: new[] { "PostId", "UserId" },
                unique: true);


            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_CreateDate",
                table: "Comments",
                columns: new[] { "PostId", "CreateDate" });


            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerUserId_FollowingUserId",
                table: "Follows",
                columns: new[] { "FollowerUserId", "FollowingUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_Status",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CreateDate",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Status_CreateDate",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Likes_PostId_UserId",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_PostId_UserId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId_CreateDate",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowerUserId_FollowingUserId",
                table: "Follows");
        }
    }
}
