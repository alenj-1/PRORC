using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Entities.Users;

namespace PRORC.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.Property(o => o.ReservationId)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.Status)
                .IsRequired();

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Reservation>()
                .WithMany()
                .HasForeignKey(o => o.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}