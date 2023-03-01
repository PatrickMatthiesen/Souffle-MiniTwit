using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLatestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Latests",
                table: "Latests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Latests");

            migrationBuilder.AddColumn<int>(
                name: "IdDub",
                table: "Latests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Latests",
                table: "Latests",
                column: "IdDub");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Latests",
                table: "Latests");

            migrationBuilder.DropColumn(
                name: "IdDub",
                table: "Latests");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Latests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Latests",
                table: "Latests",
                column: "Id");
        }
    }
}
