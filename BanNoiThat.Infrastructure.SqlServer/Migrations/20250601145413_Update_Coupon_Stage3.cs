using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Update_Coupon_Stage3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Coupons",
                newName: "CouponCode");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountAmount",
                table: "CouponUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "CouponUsages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "CouponUsages");

            migrationBuilder.RenameColumn(
                name: "CouponCode",
                table: "Coupons",
                newName: "Code");

            migrationBuilder.AlterColumn<string>(
                name: "DiscountAmount",
                table: "CouponUsages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
