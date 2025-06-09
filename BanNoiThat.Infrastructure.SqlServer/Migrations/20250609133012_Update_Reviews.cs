using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Update_Reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductItem_Id",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductItem_Id",
                table: "Reviews",
                column: "ProductItem_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProductItems_ProductItem_Id",
                table: "Reviews",
                column: "ProductItem_Id",
                principalTable: "ProductItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProductItems_ProductItem_Id",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductItem_Id",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ProductItem_Id",
                table: "Reviews");
        }
    }
}
