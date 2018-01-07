using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TimeCalculator.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IterationTime",
                table: "JobEntities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetupTime",
                table: "JobEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IterationTime",
                table: "JobEntities");

            migrationBuilder.DropColumn(
                name: "SetupTime",
                table: "JobEntities");
        }
    }
}
