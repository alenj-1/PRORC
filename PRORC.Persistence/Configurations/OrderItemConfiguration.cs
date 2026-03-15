using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }
}
