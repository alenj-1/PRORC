using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Entities.Users;

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

            builder.Property(r => r.OwnerId)
                .IsRequired();

            builder.Property(r => r.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(r => r.CuisineType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.Address)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.IsActive)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}