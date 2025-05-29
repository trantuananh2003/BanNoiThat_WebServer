using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class PutField_ProductItems_Size : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sizes",
                table: "ProductItems");

            migrationBuilder.AddColumn<int>(
                name: "heightSize",
                table: "ProductItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "lengthSize",
                table: "ProductItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "widthSize",
                table: "ProductItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "heightSize",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "lengthSize",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "widthSize",
                table: "ProductItems");

            migrationBuilder.AddColumn<string>(
                name: "Sizes",
                table: "ProductItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
