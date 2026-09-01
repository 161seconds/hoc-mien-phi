using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace piedteam_net1_2_hocmienphi.repository.Migrations
{
    /// <inheritdoc />
    public partial class sendDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isSendAdvertising",
                table: "Users",
                newName: "IsSendAdvertising");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SendDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsSendAdvertising",
                table: "Users",
                newName: "isSendAdvertising");
        }
    }
}
