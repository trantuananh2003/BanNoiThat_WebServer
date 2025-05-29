using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentCartItems : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(o => o.Id);
            //One to one
            //Nếu này là one to one
            /*
             * Giả sử khi truy vấn ngược lại productitem muốn lấy ra có bao nhiêu người đang thêm sản phẩm này vào giỏ 
             * Thì giả sử đang mối quan hệ 1 - 1 thì đồng nghĩa mỗi product item chỉ có 1 giỏ hàng vì mối quan hệ là 1 - 1
             * Mà ta cần 1 giỏ hàng có thể thêm nhiều sản phẩm
             * 
             * Còn muốn set 1 mỗi cart riêng chỉ có được 1 product id thì set khóa chính luôn
             * 
             * 1 nhiều ở đây có thể hiểu là product item là 1 và cart item là nhiều
             */
            builder.HasOne(o => o.ProductItem)
                .WithMany()
                .HasForeignKey(x => x.ProductItem_Id);

            //One to many
            builder.HasOne(cartItem => cartItem.Cart).
                WithMany(cart => cart.CartItems).
                HasForeignKey(cartItem => cartItem.Cart_Id);
        
            //Default of quantity min 1
            builder.Property(x => x.Quantity).HasDefaultValue(1); 
        }
    }
}