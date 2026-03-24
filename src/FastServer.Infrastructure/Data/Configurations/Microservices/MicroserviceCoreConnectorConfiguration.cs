using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para MicroserviceCoreConnector
/// </summary>
public class MicroserviceCoreConnectorConfiguration : IEntityTypeConfiguration<MicroserviceCoreConnector>
{
    public void Configure(EntityTypeBuilder<MicroserviceCoreConnector> builder)
    {
        builder.ToTable("FastServer_Microservice_CoreConnector");

        builder.HasKey(e => e.MicroserviceCoreConnectorId);

        builder.Property(e => e.MicroserviceCoreConnectorId)
            .HasColumnName("fastserver_microservice_core_connector_id");

        builder.Property(e => e.CoreConnectorCredentialId)
            .HasColumnName("fastserver_core_connector_credential_id");

        builder.Property(e => e.MicroserviceId)
            .HasColumnName("fastserver_microservice_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        // Relaciones
        builder.HasOne(e => e.CoreConnectorCredential)
            .WithMany(c => c.MicroserviceCoreConnectors)
            .HasForeignKey(e => e.CoreConnectorCredentialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.MicroserviceRegister)
            .WithMany(m => m.MicroserviceCoreConnectors)
            .HasForeignKey(e => e.MicroserviceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(e => e.MicroserviceId)
            .HasDatabaseName("IX_FastServer_Microservice_CoreConnector_MicroserviceId");
        builder.HasIndex(e => e.CoreConnectorCredentialId)
            .HasDatabaseName("IX_FastServer_Microservice_CoreConnector_CredentialId");
    }
}
