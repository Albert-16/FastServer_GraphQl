using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para ActivityLog
/// </summary>
public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.ToTable("activity_logs");

        builder.HasKey(e => e.ActivityLogId);

        builder.Property(e => e.ActivityLogId)
            .HasColumnName("activity_log_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.EventTypeId)
            .HasColumnName("event_type_id");

        builder.Property(e => e.ActivityLogEntityName)
            .HasColumnName("activity_log_entity_name")
            .HasMaxLength(255);

        builder.Property(e => e.ActivityLogEntityId)
            .HasColumnName("activity_log_entity_id");

        builder.Property(e => e.ActivityLogDescription)
            .HasColumnName("activity_log_description")
            .HasMaxLength(2000);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        // Relaciones
        builder.HasOne(e => e.EventType)
            .WithMany(et => et.ActivityLogs)
            .HasForeignKey(e => e.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany(u => u.ActivityLogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices simples
        builder.HasIndex(e => e.EventTypeId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.CreateAt);
        builder.HasIndex(e => e.ActivityLogEntityName);

        // Índice compuesto para búsquedas por usuario con ordenamiento descendente
        builder.HasIndex(e => new { e.UserId, e.CreateAt })
            .IsDescending(false, true)
            .HasDatabaseName("IX_ActivityLog_UserId_CreateAt_Desc");
    }
}
