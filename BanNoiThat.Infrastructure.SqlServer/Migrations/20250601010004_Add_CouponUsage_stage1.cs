using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Add_CouponUsage_stage1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CouponUsages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Coupon_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Order_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountAmount = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Coupons_Coupon_Id",
                        column: x => x.Coupon_Id,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_Coupon_Id",
                table: "CouponUsages",
                column: "Coupon_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_User_Id",
                table: "CouponUsages",
                column: "User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponUsages");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Coupons");
        }
    }
}
