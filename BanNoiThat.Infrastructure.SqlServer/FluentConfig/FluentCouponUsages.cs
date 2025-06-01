using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentCouponUsages : IEntityTypeConfiguration<CouponUsage>
    {
        public void Configure(EntityTypeBuilder<CouponUsage> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasOne(x => x.Coupon).WithMany(x => x.CouponUsages).HasForeignKey(x => x.Coupon_Id);
            builder.HasOne(x => x.User).WithMany(x => x.CouponUsages).HasForeignKey(x => x.User_Id);

        }   
    }
}
