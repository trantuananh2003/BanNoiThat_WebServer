using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentOrderItems : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.Order).WithMany(x => x.OrderItems).HasForeignKey(o => o.Order_Id);
            builder.Property(o => o.Quantity).HasDefaultValue(1);
            builder.HasOne(o => o.ProductItem).WithMany().HasForeignKey(o => o.ProductItem_Id);
            builder.Property(o => o.NameItem).HasColumnType("nvarchar(255)");
        }
    }
}
