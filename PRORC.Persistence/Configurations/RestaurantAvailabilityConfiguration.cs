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

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.RestaurantId)
                .IsRequired();

            builder.Property(a => a.AvailableDate)
                .IsRequired();

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.EndTime)
                .IsRequired();

            builder.Property(a => a.AvailableTables)
                .IsRequired();
        }
    }
}
