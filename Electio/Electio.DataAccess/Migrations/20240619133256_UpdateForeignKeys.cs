using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electio.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentsOnCourses_CourseId",
                table: "StudentsOnCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsOnCourses_StudentId",
                table: "StudentsOnCourses",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsOnCourses_Courses_CourseId",
                table: "StudentsOnCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsOnCourses_Students_StudentId",
                table: "StudentsOnCourses",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsOnCourses_Courses_CourseId",
                table: "StudentsOnCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsOnCourses_Students_StudentId",
                table: "StudentsOnCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentsOnCourses_CourseId",
                table: "StudentsOnCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentsOnCourses_StudentId",
                table: "StudentsOnCourses");
        }
    }
}
