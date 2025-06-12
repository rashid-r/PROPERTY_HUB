
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace BOOLOGAM.Infrastructure.Db_Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Feedback> PropertyFeedbacks { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<RazorPay> RazorPay {  get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        //public DbSet<UserPreference> UserPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);


            modelBuilder.Entity<Property>()
                .Property(c => c.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Property>()
                .HasOne(p => p.User)
                .WithMany(u => u.Properties)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Property>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Property>()
                .Property(p => p.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Property>()
                .HasOne(p => p.Location)
                .WithMany()
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(c => c.UserProfile)
                .WithOne()
                .HasForeignKey<UserProfile>(n => n.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<UserProfile>()
                .Property(up => up.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<UserProfile>()
                .Property(up => up.KycStatus)
                .HasConversion<string>();

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.user)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.property)
                .WithMany(p => p.WishLists)
                .HasForeignKey(w => w.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
