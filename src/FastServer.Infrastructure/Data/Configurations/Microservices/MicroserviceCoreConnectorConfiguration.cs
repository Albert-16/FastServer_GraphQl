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
        builder.ToTable("microservice_core_connector");

        builder.HasKey(e => e.MicroserviceCoreConnectorId);

        builder.Property(e => e.MicroserviceCoreConnectorId)
            .HasColumnName("microservice_core_connector_id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CoreConnectorCredentialId)
            .HasColumnName("core_connector_credential_id");

        builder.Property(e => e.MicroserviceId)
            .HasColumnName("microservice_id");

        builder.Property(e => e.CreateAt)
            .HasColumnName("create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("modify_at");

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
        builder.HasIndex(e => e.MicroserviceId);
        builder.HasIndex(e => e.CoreConnectorCredentialId);
    }
}
