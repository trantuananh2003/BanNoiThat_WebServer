using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Update_Reviews_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderItem_Id",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsComment",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_OrderItem_Id",
                table: "Reviews",
                column: "OrderItem_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_OrderItems_OrderItem_Id",
                table: "Reviews",
                column: "OrderItem_Id",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_OrderItems_OrderItem_Id",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_OrderItem_Id",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "NameUser",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "OrderItem_Id",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsComment",
                table: "OrderItems");
        }
    }
}
