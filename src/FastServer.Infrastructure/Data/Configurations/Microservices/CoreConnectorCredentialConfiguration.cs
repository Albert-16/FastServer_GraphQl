using FastServer.Domain.Entities.Microservices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastServer.Infrastructure.Data.Configurations.Microservices;

/// <summary>
/// Configuración EF Core para CoreConnectorCredential
/// </summary>
public class CoreConnectorCredentialConfiguration : IEntityTypeConfiguration<CoreConnectorCredential>
{
    public void Configure(EntityTypeBuilder<CoreConnectorCredential> builder)
    {
        builder.ToTable("FastServer_CoreConnector_Credential");

        builder.HasKey(e => e.CoreConnectorCredentialId);

        builder.Property(e => e.CoreConnectorCredentialId)
            .HasColumnName("fastserver_core_connector_credential_id");

        builder.Property(e => e.CoreConnectorCredentialUser)
            .HasColumnName("fastserver_core_connector_credential_user")
            .HasMaxLength(255);

        builder.Property(e => e.CoreConnectorCredentialPass)
            .HasColumnName("fastserver_core_connector_credential_pass")
            .HasMaxLength(500);

        builder.Property(e => e.CoreConnectorCredentialKey)
            .HasColumnName("fastserver_core_connector_credential_key")
            .HasMaxLength(500);

        builder.Property(e => e.CreateAt)
            .HasColumnName("fastserver_create_at");

        builder.Property(e => e.ModifyAt)
            .HasColumnName("fastserver_modify_at");

        // Relaciones
        builder.HasMany(e => e.MicroserviceCoreConnectors)
            .WithOne(c => c.CoreConnectorCredential)
            .HasForeignKey(c => c.CoreConnectorCredentialId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(e => e.CoreConnectorCredentialUser)
            .IsUnique()
            .HasDatabaseName("UX_FastServer_CoreConnector_Credential_User");
    }
}
