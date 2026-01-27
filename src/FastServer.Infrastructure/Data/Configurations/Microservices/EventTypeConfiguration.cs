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
        builder.ToTable("event_types");

        builder.HasKey(e => e.EventTypeId);

        builder.Property(e => e.EventTypeId)
            .HasColumnName("event_type_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.EventTypeDescription)
            .HasColumnName("event_type_description")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        // Relaciones
        builder.HasMany(e => e.ActivityLogs)
            .WithOne(a => a.EventType)
            .HasForeignKey(a => a.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.EventTypeDescription);
    }
}
