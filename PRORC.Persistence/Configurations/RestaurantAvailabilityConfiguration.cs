using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Restaurants;

namespace PRORC.Persistence.Configurations
{
    public class RestaurantAvailabilityConfiguration : IEntityTypeConfiguration<RestaurantAvailability>
    {
        public void Configure(EntityTypeBuilder<RestaurantAvailability> builder)
        {
            builder.ToTable("RestaurantAvailabilities");

            builder.HasKey(ra => ra.Id);

            builder.Property(ra => ra.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ra => ra.RestaurantId)
                .IsRequired();

            builder.Property(ra => ra.AvailableDate)
                .IsRequired();

            builder.Property(ra => ra.StartTime)
                .IsRequired();

            builder.Property(ra => ra.EndTime)
                .IsRequired();

            builder.Property(ra => ra.AvailableTables)
                .IsRequired();

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(ra => ra.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}