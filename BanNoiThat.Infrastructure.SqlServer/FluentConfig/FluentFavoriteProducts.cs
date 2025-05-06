using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentFavoriteProducts : IEntityTypeConfiguration<FavoriteProducts>
    {
        public void Configure(EntityTypeBuilder<FavoriteProducts> builder)
        {
            builder.HasKey(o => o.Id);

            //One to many
            builder.HasOne(x => x.User).WithMany(z => z.LikesProduct).HasForeignKey(x => x.User_Id);
            builder.HasOne(x => x.Product).WithMany(z => z.LikesProduct).HasForeignKey(x => x.Product_Id);
        }
    }
}
