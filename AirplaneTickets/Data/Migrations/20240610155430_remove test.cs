using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirplaneTickets.Migrations
{
    /// <inheritdoc />
    public partial class removetest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "check",
                table: "UserAccounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "check",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
