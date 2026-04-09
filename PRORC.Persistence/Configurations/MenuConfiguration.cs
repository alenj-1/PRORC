using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Entities.Restaurants;

namespace PRORC.Persistence.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menus");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.RestaurantId)
                .IsRequired();

            builder.Property(m => m.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(m => m.IsActive)
                .IsRequired();

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey(m => m.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}