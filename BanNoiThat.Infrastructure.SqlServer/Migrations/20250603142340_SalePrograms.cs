using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SalePrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaleProgram_Id",
                table: "ProductItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalePrograms",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountValue = table.Column<double>(type: "float", nullable: false),
                    MaxDiscount = table.Column<double>(type: "float", nullable: false),
                    ApplyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplyValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalePrograms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_SaleProgram_Id",
                table: "ProductItems",
                column: "SaleProgram_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_SalePrograms_SaleProgram_Id",
                table: "ProductItems",
                column: "SaleProgram_Id",
                principalTable: "SalePrograms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductItems_SalePrograms_SaleProgram_Id",
                table: "ProductItems");

            migrationBuilder.DropTable(
                name: "SalePrograms");

            migrationBuilder.DropIndex(
                name: "IX_ProductItems_SaleProgram_Id",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "SaleProgram_Id",
                table: "ProductItems");
        }
    }
}
