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
            builder.HasOne(o => o.ProductItem).WithOne().HasForeignKey<CartItem>(o => o.ProductItem_Id);

            //One to many
            builder.HasOne(cartItem => cartItem.Cart).
                WithMany(cart => cart.CartItems).
                HasForeignKey(cartItem => cartItem.Cart_Id);
        
            //Default of quantity min 1
            builder.Property(x => x.Quantity).HasDefaultValue(1); 
        }
    }
}