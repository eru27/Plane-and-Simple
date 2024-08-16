using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirplaneTickets.Migrations
{
    /// <inheritdoc />
    public partial class addedusertype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserAccounts");
        }
    }
}
