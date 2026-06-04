using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGym.API.Migrations
{
    /// <inheritdoc />
    public partial class Fieldrefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DissolvedAt",
                table: "MemberTrainers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "Enrollments",
                table: "GymClasse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DissolvedAt",
                table: "MemberTrainers");

            migrationBuilder.DropColumn(
                name: "Enrollments",
                table: "GymClasse");
        }
    }
}
