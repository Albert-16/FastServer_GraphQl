using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para User
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.UserId);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserPeoplesoft)
            .HasColumnName("user_peoplesoft")
            .HasMaxLength(100);

        builder.Property(e => e.UserActive)
            .HasColumnName("user_active");

        builder.Property(e => e.UserName)
            .HasColumnName("user_name")
            .HasMaxLength(255);

        builder.Property(e => e.UserEmail)
            .HasColumnName("user_email")
            .HasMaxLength(255);

        builder.Property(e => e.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(500);

        builder.Property(e => e.LastLogin)
            .HasColumnName("last_login");

        builder.Property(e => e.PasswordChangedAt)
            .HasColumnName("password_changed_at");

        builder.Property(e => e.EmailConfirmed)
            .HasColumnName("email_confirmed")
            .HasDefaultValue(false);

        builder.Property(e => e.RefreshToken)
            .HasColumnName("refresh_token")
            .HasMaxLength(500);

        builder.Property(e => e.RefreshTokenExpiryTime)
            .HasColumnName("refresh_token_expiry_time");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        // Relaciones
        builder.HasMany(e => e.ActivityLogs)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.UserEmail).IsUnique();
        builder.HasIndex(e => e.UserPeoplesoft);
        // Índice en UserActive eliminado - baja selectividad (booleano)
    }
}
