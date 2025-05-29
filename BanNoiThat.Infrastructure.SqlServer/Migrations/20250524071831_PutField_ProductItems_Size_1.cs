using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class PutField_ProductItems_Size_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "widthSize",
                table: "ProductItems",
                newName: "WidthSize");

            migrationBuilder.RenameColumn(
                name: "lengthSize",
                table: "ProductItems",
                newName: "LongSize");

            migrationBuilder.RenameColumn(
                name: "heightSize",
                table: "ProductItems",
                newName: "HeightSize");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WidthSize",
                table: "ProductItems",
                newName: "widthSize");

            migrationBuilder.RenameColumn(
                name: "LongSize",
                table: "ProductItems",
                newName: "lengthSize");

            migrationBuilder.RenameColumn(
                name: "HeightSize",
                table: "ProductItems",
                newName: "heightSize");
        }
    }
}
