using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Reviews;

namespace PRORC.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.RestaurantId)
                .IsRequired();

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.Property(r => r.CreatedAt)
                .IsRequired();
        }
    }
}
