using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentProductConfigs : IEntityTypeConfiguration<ProductConfig>
    { 
        public void Configure(EntityTypeBuilder<ProductConfig> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.Product).WithMany(x => x.Configs).HasForeignKey(o => o.Product_Id);
            builder.Property(o => o.Value).HasColumnType("nvarchar(255)");
        }
    }
}
