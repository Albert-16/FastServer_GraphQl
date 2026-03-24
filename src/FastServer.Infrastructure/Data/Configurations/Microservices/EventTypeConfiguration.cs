using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para EventType
/// </summary>
public class EventTypeConfiguration : IEntityTypeConfiguration<EventType>
{
    public void Configure(EntityTypeBuilder<EventType> builder)
    {
        builder.ToTable("FastServer_EventType");

        builder.HasKey(e => e.EventTypeId);

        builder.Property(e => e.EventTypeId)
            .HasColumnName("fastserver_event_type_id");

        builder.Property(e => e.EventTypeDescription)
            .HasColumnName("fastserver_event_type_description")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        // Relaciones
        builder.HasMany(e => e.ActivityLogs)
            .WithOne(a => a.EventType)
            .HasForeignKey(a => a.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.EventTypeDescription)
            .HasDatabaseName("IX_FastServer_EventType_Description");
    }
}
