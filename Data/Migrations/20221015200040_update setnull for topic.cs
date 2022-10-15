using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class updatesetnullfortopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Topic_TopicId",
                table: "Post");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Topic_TopicId",
                table: "Post",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Topic_TopicId",
                table: "Post");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Topic_TopicId",
                table: "Post",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id");
        }
    }
}
