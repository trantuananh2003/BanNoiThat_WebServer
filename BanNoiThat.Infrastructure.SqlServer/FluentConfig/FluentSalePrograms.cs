using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentSalePrograms : IEntityTypeConfiguration<SaleProgram>
    {
        public void Configure(EntityTypeBuilder<SaleProgram> builder)
        {
            builder.HasKey(o => o.Id);
            
        }   
    }
}
