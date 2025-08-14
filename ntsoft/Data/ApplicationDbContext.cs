using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ntsoft.Model;

namespace ntsoft.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(e =>
            {
                e.Property(p => p.Name)
                    .HasMaxLength(120)
                    .IsRequired();


                e.Property(p => p.Price)
                    .HasPrecision(18, 2);

                e.Property(p => p.OwnerUserId)
                    .IsRequired();

                e.HasIndex(p => p.OwnerUserId);
            });

            builder.Entity<Order>(e =>
            {

                e.Property(o => o.CustomerUserId)
                    .HasColumnName("BuyerUserId")   
                    .IsRequired();

                e.Property(o => o.Quantity).IsRequired();
                e.Property(o => o.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                e.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(o => o.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(o => o.CustomerUserId);
                e.HasIndex(o => o.ProductId);
            });

        }
    }
}
