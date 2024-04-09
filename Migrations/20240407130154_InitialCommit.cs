using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarManagement.Migrations
{
    public partial class InitialCommit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "powertbl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    volt = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Ampere = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    power = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    datetimecreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_powertbl", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "powertbl");
        }
    }
}
