using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EDPProjectGrp2.Migrations
{
    /// <inheritdoc />
    public partial class initialcreatenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    MembershipStatus = table.Column<string>(type: "longtext", nullable: false),
                    MobileNumber = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    ProfilePhotoFile = table.Column<string>(type: "longtext", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "longtext", nullable: false),
                    OccupationType = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    NewsletterSubscriptionStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorAuthStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VerificationStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ForumPost",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostTopic = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    Votes = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateEdited = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPost", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_ForumPost_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_UserId",
                table: "ForumPost",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPost");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
