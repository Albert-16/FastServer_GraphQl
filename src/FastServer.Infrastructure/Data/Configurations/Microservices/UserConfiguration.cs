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
        builder.ToTable("FastServer_User");

        builder.HasKey(e => e.UserId);

        builder.Property(e => e.UserId)
            .HasColumnName("fastserver_user_id");

        builder.Property(e => e.UserPeoplesoft)
            .HasColumnName("fastserver_user_peoplesoft")
            .HasMaxLength(100);

        builder.Property(e => e.UserActive)
            .HasColumnName("fastserver_user_active");

        builder.Property(e => e.UserName)
            .HasColumnName("fastserver_user_name")
            .HasMaxLength(255);

        builder.Property(e => e.UserEmail)
            .HasColumnName("fastserver_user_email")
            .HasMaxLength(255);

        builder.Property(e => e.PasswordHash)
            .HasColumnName("fastserver_password_hash")
            .HasMaxLength(500);

        builder.Property(e => e.LastLogin)
            .HasColumnName("fastserver_last_login");

        builder.Property(e => e.PasswordChangedAt)
            .HasColumnName("fastserver_password_changed_at");

        builder.Property(e => e.EmailConfirmed)
            .HasColumnName("fastserver_email_confirmed")
            .HasDefaultValue(false);

        builder.Property(e => e.RefreshToken)
            .HasColumnName("fastserver_refresh_token")
            .HasMaxLength(500);

        builder.Property(e => e.RefreshTokenExpiryTime)
            .HasColumnName("fastserver_refresh_token_expiry_time");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        // Relaciones
        builder.HasMany(e => e.ActivityLogs)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.UserEmail)
            .IsUnique()
            .HasDatabaseName("UX_FastServer_User_Email");
        builder.HasIndex(e => e.UserPeoplesoft)
            .HasDatabaseName("IX_FastServer_User_Peoplesoft");
    }
}
