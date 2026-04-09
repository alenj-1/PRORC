using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Entities.Users;

namespace PRORC.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.RestaurantId)
                .IsRequired();

            builder.Property(r => r.ReservationDate)
                .IsRequired();

            builder.Property(r => r.ReservationTime)
                .IsRequired();

            builder.Property(r => r.NumberOfTables)
                .IsRequired();

            builder.Property(r => r.Status)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(r => r.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}