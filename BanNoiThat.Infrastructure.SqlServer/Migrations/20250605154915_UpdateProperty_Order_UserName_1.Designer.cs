﻿// <auto-generated />
using System;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BanNoiThat.Infrastructure.SqlServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250605154915_UpdateProperty_Order_UserName_1")]
    partial class UpdateProperty_Order_UserName_1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Brand", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Cart", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("User_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.CartItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Cart_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductItem_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("Cart_Id");

                    b.HasIndex("ProductItem_Id");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryUrlImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsShow")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Parent_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Parent_Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Coupon", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Categories")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiscountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DiscountValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MaxDiscount")
                        .HasColumnType("float");

                    b.Property<double>("MinCouponValue")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TypeCoupon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsageLimit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.CouponUsage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Coupon_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("float");

                    b.Property<string>("Order_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UsageDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("User_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Coupon_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("CouponUsages");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.FavoriteProducts", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Product_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("User_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.HasIndex("User_Id");

                    b.ToTable("FavoriteProducts");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AddressCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderPaidTime")
                        .HasColumnType("datetime");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<string>("TransferService")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageItemUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameItem")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Order_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductItem_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("Order_Id");

                    b.HasIndex("ProductItem_Id");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Brand_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Category_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Keyword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThumbnailUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Brand_Id");

                    b.HasIndex("Category_Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.ProductConfig", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Product_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.ToTable("ProductConfigs");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.ProductItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Colors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("HeightSize")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LengthSize")
                        .HasColumnType("int");

                    b.Property<string>("ModelUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameOption")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Product_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<double>("SalePrice")
                        .HasColumnType("float");

                    b.Property<string>("SaleProgram_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.Property<int?>("WidthSize")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Product_Id");

                    b.HasIndex("SaleProgram_Id");

                    b.ToTable("ProductItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameNormalized")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.RoleClaim", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClaimType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Role_Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.SaleProgram", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplyType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplyValues")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiscountType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DiscountValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<double>("MaxDiscount")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SalePrograms");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool?>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMale")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role_Id")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Role_Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.CartItem", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("Cart_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BanNoiThat.Domain.Entities.ProductItem", "ProductItem")
                        .WithMany()
                        .HasForeignKey("ProductItem_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("ProductItem");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Category", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("Parent_Id");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.CouponUsage", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Coupon", "Coupon")
                        .WithMany("CouponUsages")
                        .HasForeignKey("Coupon_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BanNoiThat.Domain.Entities.User", "User")
                        .WithMany("CouponUsages")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.FavoriteProducts", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Product", "Product")
                        .WithMany("LikesProduct")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BanNoiThat.Domain.Entities.User", "User")
                        .WithMany("LikesProduct")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("Order_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BanNoiThat.Domain.Entities.ProductItem", "ProductItem")
                        .WithMany()
                        .HasForeignKey("ProductItem_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("ProductItem");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Product", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("Brand_Id")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BanNoiThat.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("Category_Id")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.ProductConfig", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Product", "Product")
                        .WithMany("Configs")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.ProductItem", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Product", "Product")
                        .WithMany("ProductItems")
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("BanNoiThat.Domain.Entities.SaleProgram", "SaleProgram")
                        .WithMany("ProductItems")
                        .HasForeignKey("SaleProgram_Id");

                    b.Navigation("Product");

                    b.Navigation("SaleProgram");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.RoleClaim", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Role", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.User", b =>
                {
                    b.HasOne("BanNoiThat.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("Role_Id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Category", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Coupon", b =>
                {
                    b.Navigation("CouponUsages");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Product", b =>
                {
                    b.Navigation("Configs");

                    b.Navigation("LikesProduct");

                    b.Navigation("ProductItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.Role", b =>
                {
                    b.Navigation("RoleClaims");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.SaleProgram", b =>
                {
                    b.Navigation("ProductItems");
                });

            modelBuilder.Entity("BanNoiThat.Domain.Entities.User", b =>
                {
                    b.Navigation("CouponUsages");

                    b.Navigation("LikesProduct");
                });
#pragma warning restore 612, 618
        }
    }
}
