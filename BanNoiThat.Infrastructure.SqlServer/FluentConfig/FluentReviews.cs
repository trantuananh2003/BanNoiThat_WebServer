using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentReviews : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(x => x.Product).WithMany(x => x.Reviews).HasForeignKey(x => x.Product_Id);
            builder.HasOne(x => x.ProductItem).WithMany().HasForeignKey(x => x.ProductItem_Id);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.User_Id);
            builder.HasOne(x => x.OrderItem).WithMany().HasForeignKey(x => x.OrderItem_Id).OnDelete(DeleteBehavior.NoAction);
           
        }
    }
}
