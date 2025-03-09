using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentCategories : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).IsRequired();
            builder.HasOne(o => o.Parent).WithMany(x => x.Children).HasForeignKey(o => o.Parent_Id);
        }
    }
}
