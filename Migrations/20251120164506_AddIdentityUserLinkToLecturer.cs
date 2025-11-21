using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityUserLinkToLecturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Lecturers",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Claims",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_IdentityUserId",
                table: "Lecturers",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_AspNetUsers_IdentityUserId",
                table: "Lecturers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_AspNetUsers_IdentityUserId",
                table: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_IdentityUserId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Claims");
        }
    }
}
