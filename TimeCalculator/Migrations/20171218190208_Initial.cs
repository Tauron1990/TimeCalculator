using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TimeCalculator.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JopEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<long>(nullable: false),
                    BigProblem = table.Column<bool>(nullable: false),
                    EffectiveTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Iterations = table.Column<long>(nullable: false),
                    NormaizedTime = table.Column<TimeSpan>(nullable: false),
                    Problem = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JopEntities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JopEntities");
        }
    }
}
