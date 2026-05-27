using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartGym.API.Migrations
{
    /// <inheritdoc />
    public partial class FieldIncorretEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrollmentDate",
                table: "Employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "EnrollmentDate",
                table: "Employee",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
