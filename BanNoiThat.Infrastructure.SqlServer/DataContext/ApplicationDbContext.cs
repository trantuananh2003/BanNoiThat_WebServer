using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.FluentConfig;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<ProductConfig> ProductConfigs { get; set; }
        public DbSet<FavoriteProducts> FavoriteProducts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }
        public DbSet<SaleProgram> SalePrograms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Product>().HasKey(o => o.Id);
            modelBuilder.ApplyConfiguration(new FluentCategories());
            modelBuilder.ApplyConfiguration(new FluentProducts());
            modelBuilder.ApplyConfiguration(new FluentProductConfigs());
            modelBuilder.ApplyConfiguration(new FluentBrands());
            modelBuilder.ApplyConfiguration(new FluentProductItems());

            modelBuilder.ApplyConfiguration(new FluentCarts());
            modelBuilder.ApplyConfiguration(new FluentCartItems());

            modelBuilder.ApplyConfiguration(new FluentOrders());
            modelBuilder.ApplyConfiguration(new FluentOrderItems());

            modelBuilder.ApplyConfiguration(new FluentUsers());
            modelBuilder.ApplyConfiguration(new FluentRoles());
            modelBuilder.ApplyConfiguration(new FluentRoleClaims());

            modelBuilder.ApplyConfiguration(new FluentFavoriteProducts());

            modelBuilder.ApplyConfiguration(new FluentCoupons());
            modelBuilder.ApplyConfiguration(new FluentCouponUsages());

            modelBuilder.ApplyConfiguration(new FluentSalePrograms());

        }
    }
}