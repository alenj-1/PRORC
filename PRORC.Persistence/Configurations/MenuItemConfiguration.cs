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

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.MenuId)
                .IsRequired();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(i => i.Description)
                .HasMaxLength(500);

            builder.Property(i => i.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.IsAvailable)
                .IsRequired();
        }
    }
}
