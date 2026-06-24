using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace piedteam_net1_2_hocmienphi.repository.Migrations
{
    /// <inheritdoc />
    public partial class Meetings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meeting_Users_UserId",
                table: "Meeting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting");

            migrationBuilder.RenameTable(
                name: "Meeting",
                newName: "Meetings");

            migrationBuilder.RenameIndex(
                name: "IX_Meeting_UserId",
                table: "Meetings",
                newName: "IX_Meetings_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Users_UserId",
                table: "Meetings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Users_UserId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meetings",
                table: "Meetings");

            migrationBuilder.RenameTable(
                name: "Meetings",
                newName: "Meeting");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_UserId",
                table: "Meeting",
                newName: "IX_Meeting_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meeting",
                table: "Meeting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Meeting_Users_UserId",
                table: "Meeting",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
