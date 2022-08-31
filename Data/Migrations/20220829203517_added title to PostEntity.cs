using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class addedtitletoPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 8, 29, 19, 46, 17, 919, DateTimeKind.Utc).AddTicks(5412));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 8, 29, 19, 46, 17, 919, DateTimeKind.Utc).AddTicks(3928));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 8, 29, 19, 46, 17, 919, DateTimeKind.Utc).AddTicks(1439));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 29, 19, 46, 17, 919, DateTimeKind.Utc).AddTicks(5412),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 29, 19, 46, 17, 919, DateTimeKind.Utc).AddTicks(3928),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegistrationDate",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 8, 29, 19, 46, 17, 919, DateTimeKind.Utc).AddTicks(1439),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
