using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Passkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passkey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CredentialId = table.Column<string>(type: "VARCHAR(900)", maxLength: 900, nullable: false),
                    PublicKey = table.Column<string>(type: "VARCHAR(900)", maxLength: 900, nullable: false),
                    CredentialType = table.Column<string>(type: "VARCHAR(900)", maxLength: 900, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AaGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passkey_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passkey_UserId",
                table: "Passkey",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passkey");
        }
    }
}
