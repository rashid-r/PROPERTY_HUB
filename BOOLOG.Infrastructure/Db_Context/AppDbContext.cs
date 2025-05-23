
using BOOLOG.Domain.Model;
using BOOLOGAM.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace BOOLOGAM.Infrastructure.Db_Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options) { }
        
        public DbSet<UserEntity> UserModel { get; set; }
        public DbSet<PropertyEntity> PropertyModel { get; set; }
        public DbSet<CategoryEntity> CategoryModel { get; set; }
        public DbSet<LocationEntity> LocationModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryEntity>()
                .Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<PropertyEntity>()
                .Property(c => c.Price)
                .HasPrecision(10,2);
            modelBuilder.Entity<UserEntity>()
                .HasMany(c => c.propertyEntries)
                .WithOne(a => a.userEntities)
                .HasForeignKey(f => f.UserEntityId);

            modelBuilder.Entity<PropertyEntity>()
                .HasOne(a => a.categoryEntities)
                .WithMany(b => b.propertyEntities)
                .HasForeignKey(c => c.CategoryEntityId);

        }

    }
}
