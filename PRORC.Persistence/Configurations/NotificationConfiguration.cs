using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Entities.Users;

namespace PRORC.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Id)
                .ValueGeneratedOnAdd();

            builder.Property(n => n.UserId)
                .IsRequired();

            builder.Property(n => n.Message)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(n => n.Type)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(n => n.Read)
                .IsRequired();

            builder.Property(n => n.SentAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}