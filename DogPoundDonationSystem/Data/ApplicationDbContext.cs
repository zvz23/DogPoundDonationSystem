using DogPoundDonationSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogPoundDonationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Donation>? Donations { get; set; }
        public DbSet<DonationItem> DonationItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the one-to-many relationship between ApplicationUser and Donation
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Donations)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Donation>()
                .HasMany(d => d.Items)
                .WithOne(di => di.Donation)
                .HasForeignKey(di => di.DonationId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Donation>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<DonationItem>()
            .Property(d => d.Id)
            .ValueGeneratedOnAdd();
        }

        public DbSet<DogPoundDonationSystem.Models.DonationItem>? DonationItem { get; set; }
    }
}