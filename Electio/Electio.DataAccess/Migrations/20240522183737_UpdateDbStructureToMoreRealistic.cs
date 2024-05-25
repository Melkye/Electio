using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electio.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbStructureToMoreRealistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudyYear",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Faculty",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Specialties",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "StudyComponent",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudyYear",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Specialties",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StudyComponent",
                table: "Courses");
        }
    }
}
