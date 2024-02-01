using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace EDPProjectGrp2.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EventName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    EventDescription = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    EventCategory = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EventLocation = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    EventTicketStock = table.Column<int>(type: "int", nullable: false),
                    EventPicture = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    EventPrice = table.Column<int>(type: "int", nullable: false),
                    EventUplayMemberPrice = table.Column<int>(type: "int", nullable: false),
                    EventNtucClubPrice = table.Column<int>(type: "int", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventDuration = table.Column<int>(type: "int", nullable: false),
                    EventSale = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EventStatus = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
