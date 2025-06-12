using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentUsers : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Birthday).HasColumnType("date");
            builder.Property(o => o.FullName).HasColumnType("nvarchar(255)");
            builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.Role_Id).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
