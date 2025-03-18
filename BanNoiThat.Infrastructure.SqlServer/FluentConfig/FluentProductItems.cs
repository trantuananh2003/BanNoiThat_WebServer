using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentProductItems : IEntityTypeConfiguration<ProductItem>
    {
        public void Configure(EntityTypeBuilder<ProductItem> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.Product).WithMany(x => x.ProductItems).HasForeignKey(x => x.Product_Id).OnDelete(DeleteBehavior.SetNull);
            builder.Property(o => o.NameOption).HasColumnType("nvarchar(255)");

        }
    }
}
