using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Entities.Orders;

namespace PRORC.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Id)
                .ValueGeneratedOnAdd();

            builder.Property(oi => oi.OrderId)
                .IsRequired();

            builder.Property(oi => oi.MenuItemId)
                .IsRequired();

            builder.Property(oi => oi.ItemName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(oi => oi.Subtotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<MenuItem>()
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}