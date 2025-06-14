﻿using BanNoiThat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanNoiThat.Infrastructure.SqlServer.FluentConfig
{
    public class FluentProducts : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).HasColumnType("nvarchar(255)");
            builder.Property(o => o.Description).HasColumnType("nvarchar(max)");
            builder.Property(o => o.CreateAt).HasColumnType("date");
            builder.HasOne(o => o.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(o => o.Category_Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(o => o.Brand_Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
