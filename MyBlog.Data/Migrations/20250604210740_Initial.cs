using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    LastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')"),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    Initials = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, computedColumnSql: "CASE \r\n    WHEN \"FirstName\" IS NOT NULL AND \"LastName\" IS NOT NULL \r\n        THEN SUBSTRING(\"FirstName\" FROM 1 FOR 1) || SUBSTRING(\"LastName\" FROM 1 FOR 1)\r\n    ELSE SUBSTRING(\"Username\" FROM 1 FOR 1)\r\nEND", stored: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avatar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BlobName = table.Column<string>(type: "text", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatar_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passkey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CredentialId = table.Column<string>(type: "VARCHAR", maxLength: 900, nullable: false),
                    PublicKey = table.Column<string>(type: "VARCHAR", maxLength: 900, nullable: false),
                    CredentialType = table.Column<string>(type: "VARCHAR", maxLength: 900, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AaGuid = table.Column<string>(type: "VARCHAR", maxLength: 900, nullable: false, comment: "Specifies authenticator type (e.g Google Password Manager, iCloud Keychain etc)"),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
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

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Content = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    Topic = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DetectedLanguage = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBanLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Reason = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Action = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBanLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBanLog_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserWarning",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWarning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWarning_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostReaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReactionType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP AT TIME ZONE 'UTC')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReaction", x => x.Id);
                    table.UniqueConstraint("AK_PostReaction_PostId_UserId", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PostReaction_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostReaction_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_UserId",
                table: "Avatar",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Passkey_UserId",
                table: "Passkey",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReaction_UserId",
                table: "PostReaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id_Username",
                table: "User",
                columns: new[] { "Id", "Username" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RefreshToken",
                table: "User",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBanLog_UserId",
                table: "UserBanLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWarning_UserId",
                table: "UserWarning",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatar");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Passkey");

            migrationBuilder.DropTable(
                name: "PostReaction");

            migrationBuilder.DropTable(
                name: "UserBanLog");

            migrationBuilder.DropTable(
                name: "UserWarning");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
