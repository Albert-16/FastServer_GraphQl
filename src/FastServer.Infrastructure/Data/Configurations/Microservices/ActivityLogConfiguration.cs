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
        builder.ToTable("FastServer_ActivityLog");

        builder.HasKey(e => e.ActivityLogId);

        builder.Property(e => e.ActivityLogId)
            .HasColumnName("fastserver_activity_log_id");

        builder.Property(e => e.EventTypeId)
            .HasColumnName("fastserver_event_type_id");

        builder.Property(e => e.ActivityLogEntityName)
            .HasColumnName("fastserver_activity_log_entity_name")
            .HasMaxLength(255);

        builder.Property(e => e.ActivityLogEntityId)
            .HasColumnName("fastserver_activity_log_entity_id");

        builder.Property(e => e.ActivityLogDescription)
            .HasColumnName("fastserver_activity_log_description")
            .HasMaxLength(2000);

        builder.Property(e => e.UserId)
            .HasColumnName("fastserver_user_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

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
        builder.HasIndex(e => e.EventTypeId)
            .HasDatabaseName("IX_FastServer_ActivityLog_EventTypeId");
        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_FastServer_ActivityLog_UserId");
        builder.HasIndex(e => e.CreateAt)
            .HasDatabaseName("IX_FastServer_ActivityLog_CreateAt");
        builder.HasIndex(e => e.ActivityLogEntityName)
            .HasDatabaseName("IX_FastServer_ActivityLog_EntityName");

        // Índice compuesto para búsquedas por usuario con ordenamiento descendente
        builder.HasIndex(e => new { e.UserId, e.CreateAt })
            .IsDescending(false, true)
            .HasDatabaseName("IX_FastServer_ActivityLog_UserId_CreateAt_Desc");
    }
}
