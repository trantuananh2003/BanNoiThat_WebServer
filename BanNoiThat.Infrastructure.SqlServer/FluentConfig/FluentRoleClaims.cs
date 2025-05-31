using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentRoleClaims : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Role).WithMany(x => x.RoleClaims).HasForeignKey(x => x.Role_Id);
        }
    }
}
