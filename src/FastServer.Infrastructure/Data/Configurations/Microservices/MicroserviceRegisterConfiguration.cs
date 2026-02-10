using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para MicroserviceRegister
/// </summary>
public class MicroserviceRegisterConfiguration : IEntityTypeConfiguration<MicroserviceRegister>
{
    public void Configure(EntityTypeBuilder<MicroserviceRegister> builder)
    {
        builder.ToTable("microservice_registers");

        builder.HasKey(e => e.MicroserviceId);

        builder.Property(e => e.MicroserviceId)
            .HasColumnName("microservice_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.MicroserviceClusterId)
            .HasColumnName("microservice_cluster_id");

        builder.Property(e => e.MicroserviceName)
            .HasColumnName("microservice_name")
            .HasMaxLength(255);

        builder.Property(e => e.MicroserviceActive)
            .HasColumnName("microservice_active");

        builder.Property(e => e.MicroserviceDeleted)
            .HasColumnName("microservice_deleted");

        builder.Property(e => e.MicroserviceCoreConnection)
            .HasColumnName("microservice_core_connection");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

        builder.Property(e => e.DeleteAt)
            .HasColumnName("delete_at");

        // Relaciones
        builder.HasOne(e => e.MicroserviceCluster)
            .WithMany(c => c.MicroserviceRegisters)
            .HasForeignKey(e => e.MicroserviceClusterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.MicroserviceCoreConnectors)
            .WithOne(c => c.MicroserviceRegister)
            .HasForeignKey(c => c.MicroserviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.MicroserviceMethods)
            .WithOne(m => m.MicroserviceRegister)
            .HasForeignKey(m => m.MicroserviceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices simples
        builder.HasIndex(e => e.MicroserviceName);
        // Índices en Active y Deleted eliminados - baja selectividad (booleanos)
        builder.HasIndex(e => e.MicroserviceClusterId);

        // Índice compuesto para búsquedas por cluster y nombre
        builder.HasIndex(e => new { e.MicroserviceClusterId, e.MicroserviceName })
            .HasDatabaseName("IX_MicroserviceRegister_ClusterId_Name");
    }
}
