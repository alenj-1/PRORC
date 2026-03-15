using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Restaurants;

namespace PRORC.Persistence.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.Address)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(r => r.CuisineType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.IsActive)
                .IsRequired();

            builder.HasMany(r => r.Availabilities)
                .WithOne(a => a.Restaurant)
                .HasForeignKey(a => a.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
