using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentRoles : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}
