using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Menus;

namespace PRORC.Persistence.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");

            builder.HasKey(mi => mi.Id);

            builder.Property(mi => mi.Id)
                .ValueGeneratedOnAdd();

            builder.Property(mi => mi.MenuId)
                .IsRequired();

            builder.Property(mi => mi.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(mi => mi.Description)
                .HasMaxLength(500);

            builder.Property(mi => mi.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(mi => mi.IsAvailable)
                .IsRequired();

            builder.HasOne<Menu>()
                .WithMany()
                .HasForeignKey(mi => mi.MenuId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}