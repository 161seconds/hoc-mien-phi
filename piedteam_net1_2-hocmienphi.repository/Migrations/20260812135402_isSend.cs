using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace piedteam_net1_2_hocmienphi.repository.Migrations
{
    /// <inheritdoc />
    public partial class isSend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSendAdvertising",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSendAdvertising",
                table: "Users");
        }
    }
}
